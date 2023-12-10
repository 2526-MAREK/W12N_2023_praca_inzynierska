using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IDeviceDataService
    {
        public Task AddDevice(Device newDevice, string hubIdentifier);

        public Task<List<Device>> GetDevices();
       
    }
}
