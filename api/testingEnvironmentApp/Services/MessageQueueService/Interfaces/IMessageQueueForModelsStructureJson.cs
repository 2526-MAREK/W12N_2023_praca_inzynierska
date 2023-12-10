namespace testingEnvironmentApp.Services.MessageQueueService.Interfaces
{
    public interface IMessageQueueForModelsStructureJson : IMessageQueueService
    {
        public string GetCurrentlySelectedOptions();

        public void ChangeCurrentlySelectedOptions(string currentlySelectedOptionsTemp);
        public void InitializeQueue();
        public bool IsQueueInitialized(string queueName);
    }
}
