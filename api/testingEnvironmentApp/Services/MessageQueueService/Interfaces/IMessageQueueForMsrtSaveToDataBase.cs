namespace testingEnvironmentApp.Services.MessageQueueService.Interfaces
{
    public interface IMessageQueueForMsrtSaveToDataBase : IMessageQueueService
    {
        public void InitializeQueue();
    }
}
