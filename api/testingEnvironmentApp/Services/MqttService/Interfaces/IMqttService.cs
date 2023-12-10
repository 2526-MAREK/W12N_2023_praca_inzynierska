using MQTTnet;

namespace testingEnvironmentApp.Services.MqttService.Interfaces
{
    public interface IMqttService
    {
        //Task SubscribeAsync(List<string> topics);
        //public Task StartAsync();
        public Task AddTopicsAsync(List<string> topics);
        //public Task StartAsync();
    }
}
