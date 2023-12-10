using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory
{
    public interface IDeviceValueJsonFactory : IMessageHelper
    {
        public void GenerateMultipleSensorSystemsJson(Dictionary<string, List<string>> systemsAndSensors, string defaultValue);
        public void UpdateSensorValue(string systemName, string sensorName, string newValue);
        public (string SystemName, JArray SensorsJson) GetSystemAndSensorsFromJson(string splitJsons);
        public List<string> SplitSystemJson();
    }
}
