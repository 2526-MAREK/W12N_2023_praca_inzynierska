

using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.MqttService.Interfaces;

namespace testingEnvironmentApp.Services.MqttService
{
    public class MqttMessagePublisher : IMqttMessagePublisher
    {
        private readonly List<IMqttMessageObserver> _observers = new();

        public void AddObserver(IMqttMessageObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IMqttMessageObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObserversAsync(string topic, string message)
        {
            var notificationTasks = _observers.Select(observer => observer.UpdateAsyncMqtt(topic, message));

            // Oczekiwanie na zakończenie wszystkich zadań.
            await Task.WhenAll(notificationTasks);
        }

        // Metoda wywoływana, gdy otrzymujemy nową wiadomość z MQTT
        public async Task NewMessageReceived(string topic, string message)
        {
            await NotifyObserversAsync(topic,  message);
        }
    }
}
