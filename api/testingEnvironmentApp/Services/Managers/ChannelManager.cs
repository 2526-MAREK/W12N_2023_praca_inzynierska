using testingEnvironmentApp.Models.Devices;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.BusinessServices;
using testingEnvironmentApp.Services.DataBaseService;
using testingEnvironmentApp.Services.MqttService;
using testingEnvironmentApp.Services.MqttService.Interfaces;
using testingEnvironmentApp.Models.Alarms;
using System.Diagnostics;
using testingEnvironmentApp.Services.Managers.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.ManagerService
{

    //DeviceManager - to klasa, która łączy w sobie różne serwisy i reaguje na wiadomości z MQTT. Można to potraktować jako część warstwy logiki biznesowej,
    //ale ze względu na specyfikę zadania (reakcję na wiadomości), można również utworzyć osobny folder, np. Managers.

    public class ChannelManager : IChannelManager
    {
        private readonly IChannelFactory _channelFactory;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IMessageQueueValueDeviceForDeviceManager _messageQueueValueDeviceForDeviceManager;

        private readonly IModelsStructureJsonManager _modelsStructureJsonManager;

        private readonly IMessageQueueForMsrtSaveToDataBase _messageQueueForMsrtSaveToDataBase;





        public ChannelManager(IMessageQueueValueDeviceForDeviceManager messageQueueValueDeviceForDeviceManager, 
            IChannelFactory channelFactory,
            IServiceScopeFactory scopeFactory, IModelsStructureJsonManager modelsStructureJsonManager,
             IMessageQueueForMsrtSaveToDataBase messageQueueForMsrtSaveToDataBase)
        {
            Debug.WriteLine("Initialize Channel Manager");
            _messageQueueValueDeviceForDeviceManager = messageQueueValueDeviceForDeviceManager;
            _channelFactory = channelFactory;
            _scopeFactory = scopeFactory;
            _modelsStructureJsonManager = modelsStructureJsonManager;
            _messageQueueForMsrtSaveToDataBase = messageQueueForMsrtSaveToDataBase;
        }


        private async Task AddMsrtToDataBase(string keyToDevice, double value)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var msrtService = scope.ServiceProvider.GetService<IMsrtService>();

                await msrtService.CreateNewMsrtToChannel(_channelFactory.GetChannelByChannelKey(keyToDevice).ChannelIdentifier, value);


            }
        }


        private async Task UpdateStatusAlarmInDataBaseAndInDeviceFactory(string keyToDevice, string message1, string message2)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var alarmService = scope.ServiceProvider.GetService<IAlarmService>();

                _channelFactory.UpdateActualStatusAlarm(keyToDevice, AlarmStatus.Resolved);
                await alarmService.FindBadStatusAlarmAndChangeToOkStatusInDataBase(_channelFactory.GetChannelByChannelKey(keyToDevice).ChannelIdentifier, message1, message2);
                Debug.WriteLine("UpdateStatusAlarmInDataBaseAndInDeviceFactory<----------------------------------------------------------------------");
                _modelsStructureJsonManager.ModifyActualAlarmStatusAndSendToWebSocket(keyToDevice, _channelFactory.GetChannelByChannelKey(keyToDevice).ActualStatusAlarm);
                //tutaj wyślemy informację do frontendu, że Alarm Status został zmieniony   <---------------------------------------------------------------------------------------------------------
            }
        }

        private async Task CheckWarningAlarmInDevice(string keyToDevice)
        {
            Channel device = new Channel();
            device = _channelFactory.GetChannelByChannelKey(keyToDevice);

            bool isHighAlarm = device.Range.UrgentHiHi != null && device.Range.WarningHi != null && device.Pv >= device.Range.WarningHi && device.Pv <= device.Range.UrgentHiHi;
            bool isLowAlarm = device.Range.UrgentLoLo != null && device.Range.WarningLo != null && device.Pv >= device.Range.UrgentLoLo && device.Pv <= device.Range.WarningLo;

            if (device.ActualStatusAlarm != AlarmStatus.Urgency)
            {
                if (!isHighAlarm && !isLowAlarm && device.ActualStatusAlarm != AlarmStatus.Resolved)
                {
                    await UpdateStatusAlarmInDataBaseAndInDeviceFactory(keyToDevice, "Warning High Alarm", "Warning Low Alarm");
                }
            }


        }


        private async Task CheckUrgentAlarmInDevice(string keyToDevice)
        {
            Channel device = new Channel();
            device = _channelFactory.GetChannelByChannelKey(keyToDevice);

            if (device.IsAcknowledgedUrgentAlarm && device.ActualStatusAlarm == AlarmStatus.Urgency)
            {
                await UpdateStatusAlarmInDataBaseAndInDeviceFactory(keyToDevice, "Urgent High Alarm", "Urgent Low Alarm");
            }
        }


        private async  Task SaveAlarmToDataBase(string keyToDevice, List<Alarm> alarms)
        {

            if (alarms.Count > 0)
            {
                Debug.WriteLine("alarms.Count");
                Debug.WriteLine(alarms.Count);
                using (var scope = _scopeFactory.CreateScope())
                {
                    var alarmService = scope.ServiceProvider.GetService<IAlarmService>();
                    foreach (var alarm in alarms)
                    {
                        await alarmService.AddAlarmToDataBase(_channelFactory.GetChannelByChannelKey(keyToDevice).ChannelIdentifier, alarm);
                        Debug.WriteLine("SaveAlarmToDataBase<----------------------------------------------------------------------");
                        _modelsStructureJsonManager.ModifyActualAlarmStatusAndSendToWebSocket(keyToDevice, _channelFactory.GetChannelByChannelKey(keyToDevice).ActualStatusAlarm);
                        //wysyłamy informację do fronendu, że Alarm Status został zmieniony   <---------------------------------------------------------------------------------------------------------
                    }
                }
            }

        }


        private async Task CheckTheDevicesFromSystem(string json, string systemIdentifier)
        {
            
            if (json != null)
            {
                var sensors = JArray.Parse(json);

                // Przeszukaj tablicę w poszukiwaniu odpowiedniego sensora
                foreach (var sensor in sensors)
                {
                        if (Double.TryParse(sensor["Value"]?.ToString(), out double value))
                        {
                        string keyToDevice = systemIdentifier + "/" + sensor["Name"].ToString();

                       

                        _channelFactory.UpdatePv(keyToDevice, value);

                        List<Alarm> alarms = new List<Alarm>();
                        alarms = _channelFactory.GetChannelByChannelKey(keyToDevice).CheckAlarms().ToList();

                        await SaveAlarmToDataBase(keyToDevice, alarms);

                        await CheckUrgentAlarmInDevice(keyToDevice);

                        await CheckWarningAlarmInDevice(keyToDevice);
                    }
                }
            }
        }


        private async Task ParseMessageToSaveMsrtsToDataBase(string message)
        {

            string[] parts = message.Split(':');

            if (parts.Length == 2)
            {
                // Trimowanie wyników, aby usunąć zbędne spacje
                string keyToDevice = parts[0].Trim();
                string valueString = parts[1].Trim();
                //Debug.WriteLine(valueString);
                double value;

                if (double.TryParse(valueString, out value))
                {
                    //Debug.WriteLine(keyToDevice + " : " + value.ToString());

                    if (_channelFactory.GetChannelByChannelKey(keyToDevice).ChannelIsHistorable && (value.GetType() == typeof(double)) && !String.Equals(value, "NaN"))
                    {     
                        await AddMsrtToDataBase(keyToDevice, value);
                    }
            }
            else
            {
                Debug.WriteLine("Parsing failed.");
            }

            }
            else
            {
            Debug.WriteLine("Input data format is bad");
            }

            }


public async Task StartAsync(CancellationToken cancellationToken)
{
    Debug.WriteLine("Start Channel Manager");

    while (true)
    {
        //We can start "CheckTheDevicesFromSystem" with many threads
        string m1 = await _messageQueueValueDeviceForDeviceManager.DequeueAsync("airSystemPoint_1");

        if (m1 != null)
        {

            await CheckTheDevicesFromSystem(m1, "airSystemPoint_1");
        }

        string m2 = await _messageQueueValueDeviceForDeviceManager.DequeueAsync("collingSystemPoint_1");

        if (m2 != null)
        {

            await CheckTheDevicesFromSystem(m2, "collingSystemPoint_1");
        }

        string m3 = await _messageQueueForMsrtSaveToDataBase.DequeueAsync("normalFlowMsrt");
        if (m3 != null)
        {

            await ParseMessageToSaveMsrtsToDataBase(m3);
        }
    }
}


}
}
