using System.Collections.Concurrent;

namespace testingEnvironmentApp.Services.MessageQueueService.Interfaces
{
    /// <summary>
    /// Interfejs dla usługi kolejki wiadomości.
    /// Definiuje podstawowe operacje na kolejce, takie jak dodawanie i pobieranie wiadomości oraz zarządzanie kolejką.
    /// </summary>
    public interface IMessageQueueService
    {
        /// <summary>
        /// Zwraca listę kolekcji blokujących zawierających wiadomości.
        /// </summary>
        /// <returns>Listę kolekcji blokujących z wiadomościami.</returns>
        public List<BlockingCollection<string>> GetMessagesQueue();
        public Task EnqueueAsync(string queueName, string message);
        public Task<string> DequeueAsync(string queueName);
        public void ClearQueue(string queueName);
        public void ClearAllQueues();
    }
}
