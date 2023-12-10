using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Services.MqttService.Interfaces;

namespace testingEnvironmentApp.Services.MqttService.Helpers
{
    public class MqttMessageHelper 
    {
        public string GenerateMultipleSensorSystemsJson(Dictionary<string, List<string>> systemsAndSensors, string defaultValue)
        {
            var systemsContainer = new SystemsContainer
            {
                Systems = new List<SensorSystem>()
            };

            // Iterujemy przez wszystkie wpisy w słowniku
            foreach (var systemEntry in systemsAndSensors)
            {
                var sensorSystem = new SensorSystem
                {
                    System = systemEntry.Key,
                    Sensors = systemEntry.Value.Select(sensorName => new Sensor
                    {
                        Name = sensorName,
                        Value = defaultValue
                    }).ToList()
                };

                systemsContainer.Systems.Add(sensorSystem);
            }

            return JsonConvert.SerializeObject(systemsContainer, Formatting.Indented);
        }

        public string UpdateSensorValue(string json, string systemName, string sensorName, string newValue)
        {
            // Deserializujemy JSON do obiektu SystemsContainer
            var systemsContainer = JsonConvert.DeserializeObject<SystemsContainer>(json);

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
                    return JsonConvert.SerializeObject(systemsContainer, Formatting.Indented);
                }
            }

            return json; // Jeśli nie znajdziemy systemu lub sensora, zwracamy oryginalny JSON
        }

        public List<string> GetOnlyNameOfSystem(List<string> listOfDevice)
        {
            List<string> uniqueSystems = listOfDevice
            .Select(sensor => sensor.Split('/')[0]) // Dzielimy string i bierzemy pierwszy element (nazwę systemu)
            .Distinct() // Usuwamy duplikaty, aby uzyskać unikalne nazwy systemów
            .ToList(); // Konwertujemy wynik na List<string>

            return uniqueSystems;
        }


        public void SplitIntoTwo(string input, out string part1, out string part2)
        {
            var index = input.IndexOf('/');
            if (index == -1)
            {
                part1 = input;
                part2 = null; // Brak '/', druga część jest null
            }
            else
            {
                part1 = input.Substring(0, index);
                part2 = input.Substring(index + 1);
            }
        }


        public  List<string> SplitSystemJson(string combinedJson)
        {
            // Deserializacja głównego JSONa do obiektu JToken
            var token = JToken.Parse(combinedJson);

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

        public (string SystemName, JArray SensorsJson) GetSystemAndSensorsFromJson(string json)
        {
            var jsonObject = JObject.Parse(json);

            // Pobieranie nazwy systemu
            string systemName = jsonObject["System"].ToString();

            // Pobieranie JSON array z sensorami
            JArray sensorsArray = (JArray)jsonObject["Sensors"];

            // Zwracanie krotki (tuple) zawierającej nazwę systemu i JSONa z sensorami
            return (SystemName: systemName, SensorsJson: sensorsArray);
        }
    }
}
