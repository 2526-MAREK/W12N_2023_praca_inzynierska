using testingEnvironmentApp.Services.MqttService.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace testingEnvironmentApp.Services.MqttService.Helpers
{
    public class Sensor
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class SensorSystem
    {
        public string System { get; set; }
        public List<Sensor> Sensors { get; set; }
    }

    public class SystemsContainer
    {
        public List<SensorSystem> Systems { get; set; }
    }
}
