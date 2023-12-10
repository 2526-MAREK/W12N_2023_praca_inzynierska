using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IHubDataService
    {
        public Task AddHub(Hub newHub);
        public Task<string> GetHubIdentifierWithDeviceIdentifier(string deviceIdentifier);

        public Task<List<Hub>> GetAllHubs();
    }
}
