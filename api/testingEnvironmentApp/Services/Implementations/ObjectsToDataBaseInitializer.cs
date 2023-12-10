using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.Implementations.Interfaces;

namespace testingEnvironmentApp.Services.Implementations
{
    public class ObjectsToDataBaseInitializer : IObjectsToDataBaseInitializer
    {
        private readonly IMsrtPointService _msrtPointService;
        private readonly IHubService _hubService;
        private readonly IDeviceService _deviceService;
        private readonly IChannelService _channelService;
        private readonly IMsrtService _msrtService;
        private readonly IMsrtAssociationService _msrtAssociationService;

        public ObjectsToDataBaseInitializer(IMsrtPointService msrtPointService, IHubService hubService, IDeviceService deviceService, IChannelService channelService, IMsrtAssociationService msrtAssociationService, IMsrtService msrtService)
        {
            _msrtPointService = msrtPointService;
            _hubService = hubService;
            _deviceService = deviceService;
            _channelService = channelService;
            _msrtAssociationService = msrtAssociationService;
            _msrtService = msrtService;
        }
        public async Task InitializeObjectsToDataBase()
        {
            await MsrtPointsToDataBaseInitialize();
            await HubsToDataBaseInitialize();
            await DevicesToDataBaseInitialize();
            await ChannelsToDataBaseInitialize();
            await AddNewMsrtToChannels();
            await MsrtAssociationToDataBaseInitialize();
        }


        private async Task MsrtPointsToDataBaseInitialize()
        {
            await _msrtPointService.CreateNewMsrtPoint("airSystemPoint_1", "Air System", "System for air in engine", "Air production", "Around engine core");
            await _msrtPointService.CreateNewMsrtPoint("collingSystemPoint_1", "Cooling System", "System for cooling in engine", "Cooling production", "Around engine core");
        }


        private async Task HubsToDataBaseInitialize()
        {
            await _hubService.CreateNewHub("airIntakeHub_1", "Air Intake Hub", "Hub for air intake to engine", "Front of the engine");
            await _hubService.CreateNewHub("airOutletHub_1", "Air Outlet Hub", "Hub for air outlet to engine", "Back of the engine");

            await _hubService.CreateNewHub("coolingInEngineCoreHub_1", "Cooling System In Engine Core Hub", "Hub for cooling liquid in engine core", "In engine core");
            await _hubService.CreateNewHub("coolingAroundEngineCoreHub_1", "Cooling system around Engine Core Hub", "Hub for cooling liquid around engine core", "Around engine core");
        }

        private async Task DevicesToDataBaseInitialize()
        {
            //Air system section 
            await _deviceService.AddNewDeviceToHub("airIntakeDevice_1","airIntakeHub_1", "islandOfSensors", "Air Intake Device", "Island Of Sensors for measurement parameters of air to turbocharger", "Before turbocharger in intake pipe");
            await _deviceService.AddNewDeviceToHub("turboChargerDevice_1", "airIntakeHub_1", "turboCharger", "Turbo Charger For Air Intake", "This is part of turbocharger on the air intake side", "End of intake air pipe and begin of pipe out turbo charger");
            await _deviceService.AddNewDeviceToHub("airIntakeDevice_2", "airIntakeHub_1", "islandOfSensors", "Air Intake Device", "Island of Sensors for measuring the parameters of the turbocharger air outlet", "After turbocharger in intake pipe");
            await _deviceService.AddNewDeviceToHub("controllerOfAirSystemDevice_1", "airIntakeHub_1", "controller", "Controller Of Air System Device", "This is controller of air system to controlling parameters of air intake to engine", "In computer of engine");

            await _deviceService.AddNewDeviceToHub("airOutletDevice_1", "airOutletHub_1", "islandOfSensors", "Air Outlet Device", "Island Of Sensors for measurement parameters of air to turbocharger", "After exit of engine core in intake pipe");
            await _deviceService.AddNewDeviceToHub("turboChargerDevice_2", "airOutletHub_1", "turboCharger", "Turbo Charger For Air Outlet", "This is part of turbocharger on the air out side", "Begin of out air pipe and end of pipe out engine core");
            await _deviceService.AddNewDeviceToHub("airOutletDevice_2", "airOutletHub_1", "islandOfSensors", "Air Outlet Device", "Island of Sensors for measuring the parameters of the turbocharger air outlet", "After turbocharger in outtake pipe");

            //Colling system section 
            await _deviceService.AddNewDeviceToHub("waterCollerDevice_1", "coolingAroundEngineCoreHub_1", "islandOfSensors", "Water Coller Device", "This system is around engine core", "Around Engine Core");
            await _deviceService.AddNewDeviceToHub("engineBlockDevice_1", "coolingInEngineCoreHub_1", "islandOfSensors", "Engine Core Device", "This system is in engine core", "In Engine Core");
        }

        private async Task ChannelsToDataBaseInitialize()
        {
            //Air system section 
            await _channelService.AddNewChannelToDevice("airIntakeDevice_1", "Sensor", "pressSensor_1", "mbar", "Pressure measurement");
            await _channelService.AddNewChannelToDevice("airIntakeDevice_1", "Sensor", "tempSensor_1", "°C", "temperature measurement");

            await _channelService.AddNewChannelToDevice("airIntakeDevice_2", "Sensor", "pressSensor_2", "mbar", "Pressure measurement");
            await _channelService.AddNewChannelToDevice("airIntakeDevice_2", "Sensor", "tempSensor_2", "°C", "temperature measurement");

            await _channelService.AddNewChannelToDevice("turboChargerDevice_1", "Sensor", "rpmSensor_1", "RPM", "rpm of rotor turbo charger measurement");

            await _channelService.AddNewChannelToDevice("controllerOfAirSystemDevice_1", "Controller", "controller_1", "---", "Control a air parameter in air system");

            await _channelService.AddNewChannelToDevice("airOutletDevice_1", "Sensor", "pressSensor_3", "mbar", "Pressure measurement");
            await _channelService.AddNewChannelToDevice("airOutletDevice_1", "Sensor", "tempSensor_3", "°C", "temperature measurement");

            await _channelService.AddNewChannelToDevice("airOutletDevice_2", "Sensor", "pressSensor_4", "mbar", "Pressure measurement");
            await _channelService.AddNewChannelToDevice("airOutletDevice_2", "Sensor", "tempSensor_4", "°C", "temperature measurement");

            await _channelService.AddNewChannelToDevice("turboChargerDevice_2", "Sensor", "rpmSensor_2", "RPM", "rpm of rotor turbo charger measurement");


            //Colling system section 
            await _channelService.AddNewChannelToDevice("waterCollerDevice_1", "Sensor", "pressSensor_5", "mbar", "Pressure measurement");
            await _channelService.AddNewChannelToDevice("engineBlockDevice_1", "Sensor", "tempSensor_5", "°C", "temperature measurement");



        }

        private async Task AddNewMsrtToChannels()
        {

            //in future add msrtPointIdentfier To AddMeasurment
           await  _msrtService.CreateNewMsrtToChannel("pressSensor_1", 0);
            await _msrtService.CreateNewMsrtToChannel("pressSensor_2", 0);
            await _msrtService.CreateNewMsrtToChannel("pressSensor_3", 0);
            await _msrtService.CreateNewMsrtToChannel("pressSensor_4", 0);
            await _msrtService.CreateNewMsrtToChannel("pressSensor_5", 0);
            await _msrtService.CreateNewMsrtToChannel("tempSensor_1", 0);
            await _msrtService.CreateNewMsrtToChannel("tempSensor_2", 0);
            await _msrtService.CreateNewMsrtToChannel("tempSensor_3", 0);
            await _msrtService.CreateNewMsrtToChannel("tempSensor_4", 0);
            await _msrtService.CreateNewMsrtToChannel("tempSensor_5", 0);
            await _msrtService.CreateNewMsrtToChannel("rpmSensor_1", 0);
            await _msrtService.CreateNewMsrtToChannel("rpmSensor_2", 0);
            await _msrtService.CreateNewMsrtToChannel("controller_1", 0);

        }
        private async Task MsrtAssociationToDataBaseInitialize()
        {
            //Air system section 
            await _msrtAssociationService.CreateNewMsrtPoint("airSystemPoint_1", "airIntakeHub_1", "Additional Info");
            await _msrtAssociationService.CreateNewMsrtPoint("airSystemPoint_1", "airOutletHub_1", "Additional Info");

            //Colling system section 
            await _msrtAssociationService.CreateNewMsrtPoint("collingSystemPoint_1", "coolingInEngineCoreHub_1", "Additional Info");
            await _msrtAssociationService.CreateNewMsrtPoint("collingSystemPoint_1", "coolingAroundEngineCoreHub_1", "Additional Info");
        }
    }
}
