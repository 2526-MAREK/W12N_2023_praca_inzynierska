using System.Collections.Concurrent;
using System.Diagnostics;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.MessageQueueService
{
    /// <summary>
    /// Implementacja MessageQueueService, specyficzna dla zarządzania urządzeniami.
    /// </summary>
    public class MessageQueueValueDeviceForDeviceManager : MessageQueueService, IMessageQueueValueDeviceForDeviceManager
    {
        IChannelFactory _channelFactory;

        /// <summary>
        /// Inicjalizuje nową instancję klasy MessageQueueValueDeviceForDeviceManager z określoną fabryką kanałów.
        /// </summary>
        /// <param name="channelFactory">Fabryka do tworzenia kanałów.</param>
        public MessageQueueValueDeviceForDeviceManager(IChannelFactory channelFactory) : base(50)
        {
            _channelFactory = channelFactory;
        }

        /// <summary>
        /// Inicjalizuje kolejki dla określonych tematów.
        /// </summary>
        /// <param name="topics">Tematy, dla których mają być zainicjalizowane kolejki.</param>
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
    }
}
