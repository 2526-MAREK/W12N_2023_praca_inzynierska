namespace testingEnvironmentApp.Services.MqttService.Interfaces
{
    public interface IMqttMessageObserver
    {
        Task UpdateAsyncMqtt(string topic, string message);
    }
}
