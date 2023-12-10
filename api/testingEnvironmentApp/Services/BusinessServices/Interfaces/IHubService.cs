using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IHubService
    {
        public Task CreateNewHub(string hubIdentifier, string name, string description, string location);
        public Task<string> GetHubIdentifierWithDeviceIdentifierFromDataBase(string deviceIdentifier);

        public Task<List<Hub>> GetAllHubsFromDataBase();
    }
}
