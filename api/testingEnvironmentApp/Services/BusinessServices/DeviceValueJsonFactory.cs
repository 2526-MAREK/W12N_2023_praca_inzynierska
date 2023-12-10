using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;
using testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class DeviceValueJsonFactory : testingEnvironmentApp.Services.MessageHelper.MessageHelper, IDeviceValueJsonFactory
    {
        string jsonMessage;
        public DeviceValueJsonFactory()
       {
                jsonMessage = "nothing";
       }

        public void GenerateMultipleSensorSystemsJson(Dictionary<string, List<string>> systemsAndSensors, string defaultValue)
        {
            var systemsContainer = new SystemsContainerJsonModel
            {
                Systems = new List<SensorSystemJsonModel>()
            };

            // Iterujemy przez wszystkie wpisy w słowniku
            foreach (var systemEntry in systemsAndSensors)
            {
                var sensorSystem = new SensorSystemJsonModel
                {
                    System = systemEntry.Key,
                    Sensors = systemEntry.Value.Select(sensorName => new SensorJsonModel
                    {
                        Name = sensorName,
                        Value = defaultValue
                    }).ToList()
                };

                systemsContainer.Systems.Add(sensorSystem);
            }

            jsonMessage = JsonConvert.SerializeObject(systemsContainer, Formatting.Indented);
        }

        public void UpdateSensorValue(string systemName, string sensorName, string newValue)
        {
            // Deserializujemy JSON do obiektu SystemsContainer
            var systemsContainer = JsonConvert.DeserializeObject<SystemsContainerJsonModel>(jsonMessage);

            // Znajdujemy odpowiedni system
            var systemToUpdate = systemsContainer.Systems.FirstOrDefault(sys => sys.System == systemName);
            if (systemToUpdate != null)
            {
                // Znajdujemy sensor w systemie
                var sensorToUpdate = systemToUpdate.Sensors.FirstOrDefault(s => s.Name == sensorName);
                if (sensorToUpdate != null)
                {
                    // Aktualizujemy wartość
                    sensorToUpdate.Value = newValue;

                    // Serializujemy obiekt z powrotem do JSONa
                    jsonMessage =  JsonConvert.SerializeObject(systemsContainer, Formatting.Indented);
                }
            }
        }

        public (string SystemName, JArray SensorsJson) GetSystemAndSensorsFromJson(string splitJsons)
        {
            var jsonObject = JObject.Parse(splitJsons);

            // Pobieranie nazwy systemu
            string systemName = jsonObject["System"].ToString();

            // Pobieranie JSON array z sensorami
            JArray sensorsArray = (JArray)jsonObject["Sensors"];

            // Zwracanie krotki (tuple) zawierającej nazwę systemu i JSONa z sensorami
            return (SystemName: systemName, SensorsJson: sensorsArray);
        }

        public List<string> SplitSystemJson()
        {
            // Deserializacja głównego JSONa do obiektu JToken
            var token = JToken.Parse(jsonMessage);

            // Lista, która będzie przechowywać rozdzielone JSONy
            List<string> splitJsons = new List<string>();

            // Przechodzenie przez każdy system w głównym JSONie
            foreach (var systemToken in token["Systems"])
            {
                // Tworzenie nowego JObject z pojedynczym systemem
                JObject singleSystem = new JObject
                {
                    ["System"] = systemToken["System"],
                    ["Sensors"] = systemToken["Sensors"]
                };

                // Serializacja JObject do stringa i dodanie do listy
                splitJsons.Add(singleSystem.ToString(Formatting.Indented));
            }

            return splitJsons;
        }
    }
}
