using testingEnvironmentApp.Models.Alarms;

namespace testingEnvironmentApp.Services.Managers.Interfaces
{
    public interface IModelsStructureJsonManager
    {
        public string InitializeStructureAllObjectWithBasicInfoJson();

        public string InitializeStructureAllObjectToFastModifyJson();

        public void ModifyActualAlarmStatusAndSendToWebSocket(string keyToChannel, AlarmStatus actualAlarmStatus);
        public void ModifyActualAlarmStatusAndSendToWebSocket(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, int actualStatusAlarm);
    }
}
