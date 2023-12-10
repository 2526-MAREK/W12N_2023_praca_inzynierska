using System.Diagnostics;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;
using testingEnvironmentApp.Services.Managers.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.Managers
{
    public class ModelsStructureJsonManager : IModelsStructureJsonManager
    {
        IModelsStructureJsonFactory _modelsStructureJsonFactory;

        IMessageQueueForModelsStructureJson _messageQueueForModelsStructureJson;

        testingEnvironmentApp.Services.MessageHelper.MessageHelper messageHelper;

        public ModelsStructureJsonManager(IModelsStructureJsonFactory modelsStructureJsonFactory, IMessageQueueForModelsStructureJson messageQueueForModelsStructureJson)
        {
            _modelsStructureJsonFactory = modelsStructureJsonFactory;
            _messageQueueForModelsStructureJson = messageQueueForModelsStructureJson;
            messageHelper = new testingEnvironmentApp.Services.MessageHelper.MessageHelper();

            if (!_messageQueueForModelsStructureJson.IsQueueInitialized("normalFlowStructureAllObjectWithBasicInfoJson") && !_messageQueueForModelsStructureJson.IsQueueInitialized("normalFlowStructureAllObjectToFastModifyJson"))
            {
                _messageQueueForModelsStructureJson.InitializeQueue();
            }
        }

        public  string InitializeStructureAllObjectWithBasicInfoJson()
        {
           

            return _modelsStructureJsonFactory.GetStructureAllObjectWithBasicInfoJson();
        }

        public  string InitializeStructureAllObjectToFastModifyJson()
        {
            return _modelsStructureJsonFactory.GetStructureAllObjectToFastModifyJson();
           
        }

        public void ModifyActualAlarmStatusAndSendToWebSocket(string msrtPointIdentifier, string hubIdentifier, string deviceIdentifier, string channelIdentifier, int actualStatusAlarm)
        {
            //_modelsStructureJsonFactory.UpdateChannelActualStatusAlarm(msrtPointIdentifier, hubIdentifier, deviceIdentifier, channelIdentifier, actualStatusAlarm);
            _messageQueueForModelsStructureJson.EnqueueAsync("normalFlowStructureAllObjectToFastModifyJson", _modelsStructureJsonFactory.GetStructureAllObjectToFastModifyJson());
        }

        public void ModifyActualAlarmStatusAndSendToWebSocket(string keyToChannel, AlarmStatus actualAlarmStatus)
        {
            messageHelper.SplitIntoTwo(keyToChannel, out string msrtPointIdentifier, out string channelIdentifier);

            string actualAlarmStatusTemp = "Resolved";
            if(actualAlarmStatus == AlarmStatus.Resolved)
            {
                actualAlarmStatusTemp = "Resolved";
            }

            if(actualAlarmStatus == AlarmStatus.Warning)
            {
                actualAlarmStatusTemp = "Warning";
            }

            if(actualAlarmStatus == AlarmStatus.Urgency)
            {
                actualAlarmStatusTemp = "Urgency";
            }

            _modelsStructureJsonFactory.UpdateStatus(msrtPointIdentifier, channelIdentifier, actualAlarmStatusTemp);
            _messageQueueForModelsStructureJson.EnqueueAsync("normalFlowStructureAllObjectToFastModifyJson", _modelsStructureJsonFactory.GetStructureAllObjectToFastModifyJson());

           // Debug.WriteLine("_modelsStructureJsonFactory.GetStructureAllObjectToFastModifyJson()<------------------------------------------------------------------------------------------------------");
           // Debug.WriteLine(_modelsStructureJsonFactory.GetStructureAllObjectToFastModifyJson());
        }
    }
}


