using testingEnvironmentApp.Services.Managers;
using testingEnvironmentApp.Services.Managers.Interfaces;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.MqttService.Interfaces;

namespace testingEnvironmentApp.Services.MqttService
{
    public class MqttMessageObserver : IMqttMessageObserver
    {
        private readonly IMessageQueueService _messageQueueService;
        public MqttMessageObserver(IMessageQueueService messageQueueService)
            {
            _messageQueueService = messageQueueService;
        }

        public Task UpdateAsyncMqtt(string topic, string message)
        {
            //var deviceManager = _deviceManagerFactory.Create();
            //deviceManager.ProcessMessageFromMqtt(topic, message);

            return Task.CompletedTask;
        }
    }
}
