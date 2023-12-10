using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory
{
    public interface IModelsStructureJsonFactory
    {
        public void BuildModelsStructureJson(
        List<MsrtAssociation> associations,
        List<MsrtPoint> msrtPoints,
        List<Hub> hubs,
        List<Device> devices,
        List<Channel> channels);

        public void CreateStructureAllObjectToFastModifyJson();
        public void CreateStructureAllObjectWithBasicInfoJson();


        public void UpdateChannelIsAcknowledgeUrgentAlarm(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, bool isAcknowledgedUrgentAlarm);

        public void UpdateChannelActualStatusAlarm(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, string actualStatusAlarm);

        public void UpdateChannelIsConnectionOk(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, bool isConnectionOk);

        public string GetStructureAllObjectWithBasicInfoJson();

        public string GetStructureAllObjectToFastModifyJson();
        public void UpdateStatus(string pointIdentifier, string channelIdentifier, string newStatus);
    }
}
