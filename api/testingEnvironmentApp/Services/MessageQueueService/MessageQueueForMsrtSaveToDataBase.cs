using System.Collections.Concurrent;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.MessageQueueService
{
    public class MessageQueueForMsrtSaveToDataBase : MessageQueueService, IMessageQueueForMsrtSaveToDataBase
    {
        public MessageQueueForMsrtSaveToDataBase() : base(50)
        {
        }

        public void InitializeQueue()
        {
            _messageQueue.Add("normalFlowMsrt", new BlockingCollection<string>());
        }
    }
}
