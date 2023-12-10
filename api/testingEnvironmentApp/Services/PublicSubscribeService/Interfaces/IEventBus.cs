namespace testingEnvironmentApp.Services.PublicSubscribeService.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : class;
        void Publish<TEventBase>(TEventBase eventToPublish) where TEventBase : class;
    }
}
