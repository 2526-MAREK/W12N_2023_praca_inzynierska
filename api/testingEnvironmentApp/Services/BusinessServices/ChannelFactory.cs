using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Models.Devices;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.PublicSubscribeService.Interfaces;
using testingEnvironmentApp.Services.MessageHelper;


namespace testingEnvironmentApp.Services.BusinessServices
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly ConcurrentDictionary<string, Channel> _channels = new ConcurrentDictionary<string, Channel>();
        IEventBus _eventBus;

        private testingEnvironmentApp.Services.MessageHelper.MessageHelper messageHelper;

        //public event EventHandler<PvChangeEventArgs> OnPvChanged;
        public ChannelFactory(IEventBus eventBus)
            {
            _eventBus = eventBus;
            messageHelper = new testingEnvironmentApp.Services.MessageHelper.MessageHelper();
            }


        public void Create(string channelKey, int idChannel, AlarmRange range, bool isAcknowledgedUrgentAlarm, bool isConnectionOk, AlarmStatus actualStatusAlarm, double pV, bool channelIsHistorable)
        {
            string deviceIdentifier, channelIdentifier;
            messageHelper.SplitIntoTwo(channelKey, out deviceIdentifier, out channelIdentifier);

            //Debug.WriteLine(actualStatusAlarm);
            var device = new Channel
            {
                IdChannel = idChannel,
                ChannelIdentifier = channelIdentifier,
                ActualStatusAlarm = actualStatusAlarm,
                Range = range,
                IsAcknowledgedUrgentAlarm = isAcknowledgedUrgentAlarm,
                Pv = pV,
                IsConnectionOk = isConnectionOk,
                ChannelIsHistorable = channelIsHistorable,
            };

            _channels.TryAdd(channelKey, device);
        }

        public void UpdatePv(string keyDevice, double newPvValue)
        {
            if (_channels.TryGetValue(keyDevice, out var device))
            {
                device.Pv = newPvValue;
            }
        }

        /*public void UpdateAlarms(string keyDevice, ICollection<Alarm> alarms)
        {
            if (_devicesWithUnitPVHistoricalAndAlarm.TryGetValue(keyDevice, out var device))
            {
                device.Alarms = alarms;
            }
        }*/


        public void UpdateActualStatusAlarm(string keyDevice, AlarmStatus alarmStatus)
        {
            Debug.WriteLine("UpdateActualStatusAlarm<-----------------------------------------------------------------------------");
            if (_channels.TryGetValue(keyDevice, out var device))
            {
                device.ActualStatusAlarm = alarmStatus;
            }
            else
            {
                Debug.WriteLine("cant add new Status Alarm to devicesWithUnitPVHistoricalAndAlarm list");
            }
        }

        // Metoda zwracająca listę wszystkich urządzeń
        public List<Channel> GetAllChannels()
        {
            return _channels.Values.ToList();
        }

        public List<string> GetAllMsrtPointsAndChannelsIdentifier()
        {
            List<string> devicesSystemIdentifier = new List<string>();
            foreach (string deviceSystemIdentifier in _channels.Keys)
            {
                devicesSystemIdentifier.Add(deviceSystemIdentifier);
            }

            return devicesSystemIdentifier;
        }

        // Metoda zwracająca urządzenie na podstawie identyfikatora
        public Channel GetChannelByChannelKey(string channelKey)
        {
            _channels.TryGetValue(channelKey, out var device);
            return device;
        }
    }
}
