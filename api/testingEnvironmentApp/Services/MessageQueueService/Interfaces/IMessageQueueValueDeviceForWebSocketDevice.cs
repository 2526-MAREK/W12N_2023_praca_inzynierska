namespace testingEnvironmentApp.Services.MessageQueueService.Interfaces
{
    public interface IMessageQueueValueDeviceForWebSocketDevice : IMessageQueueService
    {
        public void InitializeQueue(List<string> topics);
        public string GetCurrentlySelectedSystem();


        public void ChangeCurrentlySelectedSystem(string currentlySelectedSystemTemp);
        
    }
}
