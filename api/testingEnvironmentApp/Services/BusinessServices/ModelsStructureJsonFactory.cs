using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class StructuredMsrtPoint
    {
        public MsrtPoint MsrtPoint { get; set; }
        public List<StructuredHub> Hubs { get; set; } = new List<StructuredHub>();
    }

    public class StructuredHub
    {
        public Hub Hub { get; set; }
        public List<StructuredDevice> Devices { get; set; } = new List<StructuredDevice>();
    }

    public class StructuredDevice
    {
        public Device Device { get; set; }
        public List<Channel> Channels { get; set; } = new List<Channel>();
    }

    public class ModelsStructureJsonFactory : testingEnvironmentApp.Services.MessageHelper.MessageHelper, IModelsStructureJsonFactory
    {
        private string basicJsonStructureFromDataBase;
        private string structureAllObjectWithBasicInfoJson;
        private string structureAllObjectToFastModifyJson;

        public ModelsStructureJsonFactory()
        {
            basicJsonStructureFromDataBase = "nothing";
            structureAllObjectWithBasicInfoJson = "nothing";
            structureAllObjectToFastModifyJson = "nothing";
        }

        public void BuildModelsStructureJson(
        List<MsrtAssociation> associations,
        List<MsrtPoint> msrtPoints,
        List<Hub> hubs,
        List<Device> devices,
        List<Channel> channels)
        {
            var msrtPointsDict = new Dictionary<int, MsrtPoint>();
            var hubsDict = new Dictionary<int, Hub>();
            var devicesDict = new Dictionary<int, Device>();
            var channelsDict = new Dictionary<int, Channel>();

            foreach (var point in msrtPoints)
                msrtPointsDict[point.IdPoint] = point;

            foreach (var hub in hubs)
                hubsDict[hub.IdHub] = hub;

            foreach (var device in devices)
                devicesDict[device.IdDevice] = device;

            foreach (var channel in channels)
                channelsDict[channel.IdChannel] = channel;

            // Struktura pomocnicza do tworzenia zagnieżdżenia
            var structuredMsrtPoints = new Dictionary<int, StructuredMsrtPoint>();

            foreach (var association in associations)
            {
                if (!structuredMsrtPoints.TryGetValue(association.IdPoint, out var structuredMsrtPoint))
                {
                    structuredMsrtPoint = new StructuredMsrtPoint { MsrtPoint = msrtPointsDict[association.IdPoint] };
                    structuredMsrtPoints[association.IdPoint] = structuredMsrtPoint;
                }

                var structuredHub = structuredMsrtPoint.Hubs.FirstOrDefault(h => h.Hub.IdHub == association.IdHubs);
                if (structuredHub == null)
                {
                    structuredHub = new StructuredHub { Hub = hubsDict[association.IdHubs] };
                    structuredMsrtPoint.Hubs.Add(structuredHub);
                }

                var structuredDevice = structuredHub.Devices.FirstOrDefault(d => d.Device.IdDevice == association.IdDevice);
                if (structuredDevice == null)
                {
                    structuredDevice = new StructuredDevice { Device = devicesDict[association.IdDevice] };
                    structuredHub.Devices.Add(structuredDevice);
                }

                if (channelsDict.TryGetValue(association.IdChannels, out var channel))
                {
                    structuredDevice.Channels.Add(channel);
                }
            }

            //Debug.WriteLine(jsonMessage);
            // Serializacja do JSON
            basicJsonStructureFromDataBase = JsonConvert.SerializeObject(structuredMsrtPoints.Values, Formatting.Indented);
        }


        public void CreateStructureAllObjectWithBasicInfoJson()
        {
            var jArray = JsonConvert.DeserializeObject<JArray>(basicJsonStructureFromDataBase);

            foreach (var item in jArray)
            {
                RemoveFields(item, new string[] { "Pv", "Range", "IsConnectionOk", "IsAcknowledgedUrgentAlarm", "ActualStatusAlarm" });
            }



            structureAllObjectWithBasicInfoJson = jArray.ToString();
        }

        private void RemoveFields(JToken token, string[] fields)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var propertiesToRemove = new List<JProperty>();
                    foreach (var child in token.Children<JProperty>())
                    {
                        if (Array.IndexOf(fields, child.Name) != -1)
                        {
                            propertiesToRemove.Add(child);
                        }
                        else
                        {
                            RemoveFields(child.Value, fields);
                        }
                    }
                    foreach (var prop in propertiesToRemove)
                    {
                        prop.Remove();
                    }
                    break;
                case JTokenType.Array:
                    foreach (var child in token.Children())
                    {
                        RemoveFields(child, fields);
                    }
                    break;
            }
        }

        public void CreateStructureAllObjectToFastModifyJson()
        {
            var jArray = JArray.Parse(basicJsonStructureFromDataBase);
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, object>>>>();

            foreach (var item in jArray)
            {
                var msrtPointId = item["MsrtPoint"]?["MsrtPointIdentifier"]?.ToString();
                if (msrtPointId != null)
                {
                    if (!result.ContainsKey(msrtPointId))
                    {
                        result[msrtPointId] = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();
                    }

                    var hubs = item["Hubs"] as JArray;
                    foreach (var hub in hubs)
                    {
                        var hubId = hub["Hub"]?["HubIdentifier"]?.ToString();
                        if (hubId != null)
                        {
                            if (!result[msrtPointId].ContainsKey(hubId))
                            {
                                result[msrtPointId][hubId] = new Dictionary<string, Dictionary<string, object>>();
                            }

                            var devices = hub["Devices"] as JArray;
                            foreach (var device in devices)
                            {
                                var deviceId = device["Device"]?["DeviceIdentifier"]?.ToString();
                                if (deviceId != null)
                                {
                                    if (!result[msrtPointId][hubId].ContainsKey(deviceId))
                                    {
                                        result[msrtPointId][hubId][deviceId] = new Dictionary<string, object>();
                                    }

                                    var channels = device["Channels"] as JArray;
                                    foreach (var channel in channels)
                                    {
                                        var channelId = channel["ChannelIdentifier"]?.ToString();
                                        if (channelId != null)
                                        {
                                            // Ustawiamy atrybuty na zadane wartości
                                            result[msrtPointId][hubId][deviceId][channelId] = new
                                            {
                                                IsAcknowledgedUrgentAlarm = true,
                                                ActualStatusAlarm = "Resolved",
                                                IsConnectionOk = true
                                            };
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            structureAllObjectToFastModifyJson = Newtonsoft.Json.JsonConvert.SerializeObject(result); 

            Debug.WriteLine("structureAllObjectToFastModifyJson");
            Debug.WriteLine(structureAllObjectToFastModifyJson);
        }

        public void UpdateStatus( string pointIdentifier, string channelIdentifier, string newStatus)
        {
            var json = JObject.Parse(structureAllObjectToFastModifyJson);

            // Sprawdzamy, czy pointIdentifier istnieje w JSON
            if (json[pointIdentifier] == null)
            {
                Console.WriteLine("Point identifier not found");
                return;
            }

            // Funkcja rekurencyjna do przeszukiwania i aktualizowania JSON
            bool UpdateRecursive(JToken token)
            {
                foreach (var property in token.Children<JProperty>())
                {
                    if (property.Name == channelIdentifier)
                    {
                        var actualStatusAlarm = property.Value["ActualStatusAlarm"];
                        if (actualStatusAlarm != null)
                        {
                            property.Value["ActualStatusAlarm"] = newStatus;
                            return true;
                        }
                    }

                    if (property.Value is JObject || property.Value is JArray)
                    {
                        if (UpdateRecursive(property.Value))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // Wywołanie metody rekurencyjnej
            UpdateRecursive(json[pointIdentifier]);

            structureAllObjectToFastModifyJson = json.ToString();
        }



        public void UpdateChannelIsAcknowledgeUrgentAlarm(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, bool isAcknowledgedUrgentAlarm)
        {
            var jObject = JObject.Parse(structureAllObjectToFastModifyJson);

            // Navigacja do odpowiedniego kanału
            var channelToken = jObject.SelectToken($"$.{msrtPointIdentifier}.{hubIdentifier}.{deviceIdentifier}.{channelIdentifier}");

            if (channelToken != null)
            {
                channelToken["IsAcknowledgedUrgentAlarm"] = isAcknowledgedUrgentAlarm;
            }
            else
            {
                throw new Exception("Specified channel not found.");
            }

            // Konwersja z powrotem na string, jeśli potrzebujesz zaktualizowanego JSON
            string updatedJson = jObject.ToString();
            // Możesz teraz zwrócić zaktualizowany JSON lub wykonać inne działania
        }

        public void UpdateChannelActualStatusAlarm(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, string actualStatusAlarm)
        {
            var jObject = JObject.Parse(structureAllObjectToFastModifyJson);

            // Navigacja do odpowiedniego kanału
            var channelToken = jObject.SelectToken($"$.{msrtPointIdentifier}.{hubIdentifier}.{deviceIdentifier}.{channelIdentifier}");

            if (channelToken != null)
            {
                channelToken["ActualStatusAlarm"] = actualStatusAlarm;
            }
            else
            {
                throw new Exception("Specified channel not found.");
            }

            // Konwersja z powrotem na string, jeśli potrzebujesz zaktualizowanego JSON
            string updatedJson = jObject.ToString();
            // Możesz teraz zwrócić zaktualizowany JSON lub wykonać inne działania
        }

        public void UpdateChannelIsConnectionOk(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, bool isConnectionOk)
        {
            var jObject = JObject.Parse(structureAllObjectToFastModifyJson);

            // Navigacja do odpowiedniego kanału
            var channelToken = jObject.SelectToken($"$.{msrtPointIdentifier}.{hubIdentifier}.{deviceIdentifier}.{channelIdentifier}");

            if (channelToken != null)
            {
                channelToken["IsConnectionOk"] = isConnectionOk;
            }
            else
            {
                throw new Exception("Specified channel not found.");
            }

            // Konwersja z powrotem na string, jeśli potrzebujesz zaktualizowanego JSON
            string updatedJson = jObject.ToString();
            // Możesz teraz zwrócić zaktualizowany JSON lub wykonać inne działania
        }

        public string GetStructureAllObjectWithBasicInfoJson()
        {
            return structureAllObjectWithBasicInfoJson;
        }


        public string GetStructureAllObjectToFastModifyJson()
        {
            return structureAllObjectToFastModifyJson;
        }
    }
}
