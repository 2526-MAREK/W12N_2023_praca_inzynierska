using testingEnvironmentApp.Services.PublicSubscribeService.Interfaces;

namespace testingEnvironmentApp.Services.PublicSubscribeService
{
    public class EventBus : IEventBus
    {
        private readonly IDictionary<Type, IList<Action<object>>> _subscriptions;

        public EventBus()
        {
            _subscriptions = new Dictionary<Type, IList<Action<object>>>();
        }

        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : class
        {
            var eventType = typeof(TEventBase);
            if (!_subscriptions.ContainsKey(eventType))
            {
                _subscriptions[eventType] = new List<Action<object>>();
            }
            _subscriptions[eventType].Add((e) => action((TEventBase)e));
        }

        public void Publish<TEventBase>(TEventBase eventToPublish) where TEventBase : class
        {
            var eventType = typeof(TEventBase);
            if (_subscriptions.ContainsKey(eventType))
            {
                foreach (var action in _subscriptions[eventType])
                {
                    action(eventToPublish);
                }
            }
        }
    }
}
