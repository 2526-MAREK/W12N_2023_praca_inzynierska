namespace testingEnvironmentApp.Services.Implementations.Interfaces
{
    public interface IObjectsInitializer
    {
        void InitializeObjects();
        public bool IsInitializationComplete();
        public Task InitializeObjectsAsync();
    }
}
