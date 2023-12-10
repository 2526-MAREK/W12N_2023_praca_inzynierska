using System.Diagnostics;
using System.Threading.Channels;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;
using testingEnvironmentApp.Services.Implementations.Interfaces;
using testingEnvironmentApp.Models;
using System.Collections.Concurrent;

namespace testingEnvironmentApp.Services.Implementations
{
    public class ObjectsInitializer : IObjectsInitializer
    {
        private readonly IChannelFactory _channelFactory;

        private readonly IDeviceValueJsonFactory _deviceValueJsonFactory;
        private readonly IModelsStructureJsonFactory _modelsStructureJsonFactory;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ConcurrentDictionary<int, string> channelIdAndKeyChannelsIdentifierList = new ConcurrentDictionary<int, string>();

        private readonly object _initializationLock = new object();
        private bool _isInitialized = false;

        public ObjectsInitializer(IChannelFactory channelFactory, IDeviceValueJsonFactory deviceValueJsonFactory, IServiceScopeFactory scopeFactory, IModelsStructureJsonFactory modelsStructureJsonFactory)
        {
            _channelFactory = channelFactory;
            _deviceValueJsonFactory = deviceValueJsonFactory;

            _scopeFactory = scopeFactory;
            _modelsStructureJsonFactory = modelsStructureJsonFactory;
        }

        public void InitializeObjects()
        {
            lock (_initializationLock)
            {
                if (!_isInitialized)
                {
                    Debug.WriteLine("Initializing devices...");
                    AlarmRange alarmRange = new AlarmRange
                    {
                        WarningHi = 100,
                        WarningLo = 50,
                        UrgentHiHi = 120,
                        UrgentLoLo = 20
                    };



                    foreach (int idChannel in channelIdAndKeyChannelsIdentifierList.Keys)
                    {
                        if (channelIdAndKeyChannelsIdentifierList.TryGetValue(idChannel, out var keyChannelIdentifier))
                        {
                            if ((keyChannelIdentifier == "airSystemPoint_1/tempSensor_1") ||
                                (keyChannelIdentifier == "airSystemPoint_1/pressSensor_1") ||
                                (keyChannelIdentifier == "collingSystemPoint_1/pressSensor_5") ||
                                (keyChannelIdentifier == "collingSystemPoint_1/tempSensor_5"))
                            {
                                _channelFactory.Create(keyChannelIdentifier, idChannel, alarmRange, true, true, AlarmStatus.Resolved, 0, true);
                            }
                            else
                            {
                                _channelFactory.Create(keyChannelIdentifier, idChannel, alarmRange, true, true, AlarmStatus.Resolved, 0, false);
                            }
                        }
                    }



                    ChannelValueJsonInitialize();
                    _modelsStructureJsonFactory.CreateStructureAllObjectToFastModifyJson();
                    _modelsStructureJsonFactory.CreateStructureAllObjectWithBasicInfoJson();
                    _isInitialized = true;
                }
            }
        }
        public async Task InitializeObjectsAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var channelService = scope.ServiceProvider.GetService<IChannelService>();
                var deviceService = scope.ServiceProvider.GetService<IDeviceService>();
                var hubService = scope.ServiceProvider.GetService<IHubService>();
                var msrtAssociationService = scope.ServiceProvider.GetService<IMsrtAssociationService>();
                var msrPointService = scope.ServiceProvider.GetService<IMsrtPointService>();

                var channelsList = await channelService.GetAllCahnnelsFromDataBase();
                //Debug.WriteLine("channelsList.Count");
                //Debug.WriteLine(channelsList.Count);

                foreach (var channel in channelsList)
                {
                    string msrtPointIdentifier;
                    msrtPointIdentifier = await msrtAssociationService.GetMsrtPointIdentifierWithChannelIdentifierFromDataBase(channel.ChannelIdentifier);
                    channelIdAndKeyChannelsIdentifierList.TryAdd(channel.IdChannel, msrtPointIdentifier + "/" + channel.ChannelIdentifier);

                }

                var mstrtAssociationList = await msrtAssociationService.GetAllMstrAssociationFromDataBase();

                _modelsStructureJsonFactory.BuildModelsStructureJson(mstrtAssociationList, await msrPointService.GetAllMstrPointsFromDataBase(),
                    await hubService.GetAllHubsFromDataBase(), await deviceService.GetDevicesFromDataBase(), channelsList);
            }
        }

        private void ChannelValueJsonInitialize()
        {
            List<string> topics;
            topics = channelIdAndKeyChannelsIdentifierList.Values.ToList();
            List<string> nameOfSystems;

            nameOfSystems = _deviceValueJsonFactory.GetOnlyNameOfSystem(topics);

            var systemsAndSensors = new Dictionary<string, List<string>>();

            foreach (string nameOfSystem in nameOfSystems)
            {
                systemsAndSensors.Add(nameOfSystem, new List<string>());
            }


            foreach (string topic in topics)
            {
                string system, sensor;
                _deviceValueJsonFactory.SplitIntoTwo(topic, out system, out sensor);
                systemsAndSensors[system].Add(sensor);
            }



            _deviceValueJsonFactory.GenerateMultipleSensorSystemsJson(systemsAndSensors, "NaN");
        }



        public bool IsInitializationComplete()
        {
            lock (_initializationLock)
            {
                return _isInitialized;
            }
        }
    }
}

