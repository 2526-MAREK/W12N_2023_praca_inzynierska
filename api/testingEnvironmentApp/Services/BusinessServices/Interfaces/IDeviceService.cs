using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Devices;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IDeviceService
    {
        public Task AddNewDeviceToHub(string deviceIdentifier, string hubIdentifier, string type, string name, string description, string location);
        public Task<List<Device>> GetDevicesFromDataBase();
    }
}
