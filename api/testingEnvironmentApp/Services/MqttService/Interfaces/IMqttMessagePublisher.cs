using System;

namespace testingEnvironmentApp.Services.MqttService.Interfaces
{
    public interface IMqttMessagePublisher
    {
        public void AddObserver(IMqttMessageObserver observer);


        public void RemoveObserver(IMqttMessageObserver observer);


        public  Task NotifyObserversAsync(string topic, string message);


        // Metoda wywoływana, gdy otrzymujemy nową wiadomość z MQTT
        public  Task NewMessageReceived(string topic, string message);
        
    }
}
