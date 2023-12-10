using System.Collections.Concurrent;
using System.Diagnostics;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;

namespace testingEnvironmentApp.Services.MessageQueueService
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla usług kolejki wiadomości.
    /// Zapewnia podstawową implementację operacji na kolejkach wiadomości.
    /// </summary>
    public abstract class MessageQueueService : IMessageQueueService
    {
        protected readonly Dictionary<string, BlockingCollection<string>> _messageQueue;
        protected readonly int MaxQueueSize;

        /// <summary>
        /// Inicjalizuje nową instancję klasy MessageQueueService z określonym maksymalnym rozmiarem kolejki.
        /// </summary>
        /// <param name="maxQueueSize">Maksymalny rozmiar kolejki.</param>
        protected MessageQueueService(int maxQueueSize)
        {
            _messageQueue = new Dictionary<string, BlockingCollection<string>>();
            MaxQueueSize = maxQueueSize;
            //InitializeQueue();
        }

        //protected abstract void InitializeQueue();
        public List<BlockingCollection<string>> GetMessagesQueue()
        {
            return new List<BlockingCollection<string>>(_messageQueue.Values);
        }

        public Task EnqueueAsync(string queueName, string message)
        {
            
            if (!_messageQueue.TryGetValue(queueName, out var queue))
            {
                queue = new BlockingCollection<string>(new ConcurrentQueue<string>(), MaxQueueSize);
                _messageQueue[queueName] = queue;
            }

            queue.Add(message); // Dodaje wiadomość do BlockingCollection
            return Task.CompletedTask;
        }

        public Task<string> DequeueAsync(string queueName)
        {

            if (_messageQueue.TryGetValue(queueName, out var queue))
            {
                var message = queue.Take(); // Blokuje wątek, dopóki nie pojawi się element
                return Task.FromResult(message);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public void ClearAllQueues()
        {
            foreach (var queue in _messageQueue.Values)
            {
                queue.CompleteAdding(); // Zamyka kolekcję do dalszych dodawań

                while (queue.TryTake(out var _))
                {
                    // Usuwa wszystkie elementy z kolejki
                }
            }

            _messageQueue.Clear(); // Czyści słownik z wszystkich kolekcji
        }


        public void ClearQueue(string queueName)
        {
            if (_messageQueue.TryGetValue(queueName, out var queue))
            {
                queue.CompleteAdding(); // Zamyka kolekcję do dalszych dodawań
                while (queue.TryTake(out var _)) { } // Usuwa wszystkie elementy

                // Resetowanie kolejki
                _messageQueue[queueName] = new BlockingCollection<string>(new ConcurrentQueue<string>(), MaxQueueSize);
            }
        }



    }
}
