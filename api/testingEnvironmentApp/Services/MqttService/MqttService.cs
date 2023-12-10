using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Diagnostics;
using System.Text;
using testingEnvironmentApp.Services.MqttService.Interfaces;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.MessageHelper;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.BusinessServices;

namespace testingEnvironmentApp.Services.MqttService
{
    public class MqttService : IMqttService, IHostedService
    {
        private IMqttClient _client;
        private IMqttClientOptions _options;

        IMessageQueueValueDeviceForWebSocketDevice _messageQueueValueDeviceForWebSocketDevice;
        IMessageQueueValueDeviceForDeviceManager _messageQueueValueDeviceForDeviceManager;
        IMessageQueueForMsrtSaveToDataBase _messageQueueForMsrtSaveToDataBase;


        IChannelFactory _channelFactory;
        IDeviceValueJsonFactory _deviceValueJsonFactory;

        private List<string> _topicsToSubscribe = new List<string>();


        public MqttService(IMessageQueueValueDeviceForWebSocketDevice messageQueueValueDeviceForWebSocketDevice,
            IMessageQueueValueDeviceForDeviceManager messageQueueValueDeviceForDeviceManager, 
            IChannelFactory channelFactory, IDeviceValueJsonFactory deviceValueJsonFactory, IMessageQueueForMsrtSaveToDataBase messageQueueForMsrtSaveToDataBase
            )//IMqttMessagePublisher messagePublisher)
        {
            _deviceValueJsonFactory = deviceValueJsonFactory;

            _messageQueueValueDeviceForWebSocketDevice = messageQueueValueDeviceForWebSocketDevice;
            _messageQueueValueDeviceForDeviceManager = messageQueueValueDeviceForDeviceManager;
            _messageQueueForMsrtSaveToDataBase = messageQueueForMsrtSaveToDataBase;
            _channelFactory = channelFactory;

            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("localhost", 1883)
                .WithCleanSession()
                .Build();

            _client.UseApplicationMessageReceivedHandler(HandleMessageReceived);
        }

        public async Task AddTopicsAsync(List<string> topics)
        {
            foreach (string topic in topics)
            {
                if (!_topicsToSubscribe.Contains(topic))
                {
                    _topicsToSubscribe.Add(topic);
                    if (_client.IsConnected)
                    {
                        await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                        Debug.WriteLine($"Subscribed to {topic}");
                    }
                }
            }
        }


       

        private async Task HandleMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
        {

            try
            {
                var topic = arg.ApplicationMessage.Topic;
                var message = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);

              
                string system, sensor;

                _deviceValueJsonFactory.SplitIntoTwo(topic, out system, out sensor);

                _deviceValueJsonFactory.UpdateSensorValue(system, sensor, message);
               
                await _messageQueueForMsrtSaveToDataBase.EnqueueAsync("normalFlowMsrt", topic + ": " + message);

                List<string> splitJsons = _deviceValueJsonFactory.SplitSystemJson();

                await SendMessagesInParallelAsync(splitJsons);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error handling received message: {ex.Message}");
            }
        }

        public async Task SendMessagesInParallelAsync(List<string> splitJsons)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < splitJsons.Count; i++)
            {
                try
                {
                    var result = _deviceValueJsonFactory.GetSystemAndSensorsFromJson(splitJsons[i]);
                    
                    if (result.SystemName == _messageQueueValueDeviceForWebSocketDevice.GetCurrentlySelectedSystem())
                    {
                  
                        await _messageQueueValueDeviceForWebSocketDevice.EnqueueAsync(result.SystemName, result.SensorsJson.ToString());
                    }

                    await _messageQueueValueDeviceForDeviceManager.EnqueueAsync(result.SystemName, result.SensorsJson.ToString());

                }
                catch (Exception ex)
                {
                    // Logowanie nieprzewidzianego błędu
                    Debug.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            try
            {
                // Oczekiwanie na zakończenie wszystkich zadań
                await Task.WhenAll(tasks);
            }
            catch (AggregateException ae)
            {
                // Obsługa błędów, gdy jedno z zadań zgłasza wyjątek
                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    Debug.WriteLine($"Task exception: {e.Message}");
                }
            }
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _client.ConnectAsync(_options);
                Debug.WriteLine("Successfully connected to MQTT broker.");

                List<string> topics;
                topics = _channelFactory.GetAllMsrtPointsAndChannelsIdentifier();

                _messageQueueValueDeviceForWebSocketDevice.InitializeQueue(topics);
                _messageQueueValueDeviceForDeviceManager.InitializeQueue(topics);
                _messageQueueForMsrtSaveToDataBase.InitializeQueue();
                
                await AddTopicsAsync(topics);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up any resources if needed
            return Task.CompletedTask;
        }

       
    }
}