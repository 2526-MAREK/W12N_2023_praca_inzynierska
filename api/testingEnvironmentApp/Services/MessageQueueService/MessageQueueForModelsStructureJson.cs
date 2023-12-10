using System.Collections.Concurrent;
using System.Diagnostics;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.MessageQueueService
{
    public class MessageQueueForModelsStructureJson : MessageQueueService, IMessageQueueForModelsStructureJson
    {
        private string currentlySelectedOptions;
        public MessageQueueForModelsStructureJson() : base(50)
        {
            currentlySelectedOptions = "initializeStructureAllObjectWithBasicInfoJson";
        }

        public void InitializeQueue()
        {
            _messageQueue.Add("normalFlowStructureAllObjectWithBasicInfoJson", new BlockingCollection<string>());
            _messageQueue.Add("normalFlowStructureAllObjectToFastModifyJson", new BlockingCollection<string>());


        }

        public string GetCurrentlySelectedOptions()
        {
            return currentlySelectedOptions;
        }

        public void ChangeCurrentlySelectedOptions(string currentlySelectedOptionsTemp)
        {
            currentlySelectedOptions = currentlySelectedOptionsTemp;
        }

        public bool IsQueueInitialized(string queueName)
        {
            return _messageQueue.ContainsKey(queueName);
        }
    }
}
