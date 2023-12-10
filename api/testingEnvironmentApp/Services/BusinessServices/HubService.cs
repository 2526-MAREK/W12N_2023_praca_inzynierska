using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class HubService : IHubService
    {
        IHubDataService _hubDataService;
        public HubService(IHubDataService hubDataService) {
            _hubDataService = hubDataService;
        }

        public async Task CreateNewHub(string hubIdentifier , string name, string description , string location )
        {
            var hub = new Hub { HubIdentifier = hubIdentifier, Name = name, Description = description, Location = location };
            await _hubDataService.AddHub(hub);
        }

        public async Task<string> GetHubIdentifierWithDeviceIdentifierFromDataBase(string deviceIdentifier)
        {
            return await _hubDataService.GetHubIdentifierWithDeviceIdentifier(deviceIdentifier);
        }

        public async Task<List<Hub>> GetAllHubsFromDataBase()
        {
            return await _hubDataService.GetAllHubs();
        }
    }
}
