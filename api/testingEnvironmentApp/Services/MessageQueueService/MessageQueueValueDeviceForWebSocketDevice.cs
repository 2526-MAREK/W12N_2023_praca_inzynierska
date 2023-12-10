using System.Collections.Concurrent;
using System.Diagnostics;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.MessageQueueService
{
    public class MessageQueueValueDeviceForWebSocketDevice : MessageQueueService, IMessageQueueValueDeviceForWebSocketDevice
    {
        private string currentlySelectedSystem;
        public MessageQueueValueDeviceForWebSocketDevice() : base(50)
        {
            currentlySelectedSystem = "nothing";
        }

        public void InitializeQueue(List<string> topics)
        {
            List<string> topicsTemp;
            topicsTemp = topics.Select(topic => topic.Split('/')[0])
                     .Distinct()
                     .ToList();

            foreach (string topic in topicsTemp)
            {
                //Debug.WriteLine(topic);
                _messageQueue.Add(topic, new BlockingCollection<string>());
            }

        }

        public string GetCurrentlySelectedSystem()
        {
            return currentlySelectedSystem;
        }

        public void ChangeCurrentlySelectedSystem(string currentlySelectedSystemTemp)
        {
            currentlySelectedSystem = currentlySelectedSystemTemp;
        }


    }
}
