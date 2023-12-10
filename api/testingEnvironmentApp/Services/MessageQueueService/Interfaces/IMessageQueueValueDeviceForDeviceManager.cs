namespace testingEnvironmentApp.Services.MessageQueueService.Interfaces
{
    /// <summary>
    /// Interfejs dla usługi kolejki wiadomości, specyficznej dla zarządzania urządzeniami.
    /// Rozszerza IMessageQueueService o metody inicjalizacji kolejek.
    /// </summary>
    public interface IMessageQueueValueDeviceForDeviceManager : IMessageQueueService
    {
        /// <summary>
        /// Inicjalizuje kolejki dla określonych tematów.
        /// </summary>
        /// <param name="topics">Tematy, dla których mają być zainicjalizowane kolejki.</param>
        public void InitializeQueue(List<string> topics);
    }
}
