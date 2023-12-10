using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Device = testingEnvironmentApp.Models.Device;
using testingEnvironmentApp.Services.DataServices.Interfaces;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    //ta klasa odpowiada za operacje na urządzeniach, więc można go zakwalifikować jako serwis biznesowy i umieścić w folderze BusinessServices.
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceDataService _deviceDataService;

        public DeviceService(IDeviceDataService deviceDataService) 
        {
            _deviceDataService = deviceDataService;
        }


        public async Task AddNewDeviceToHub(string deviceIdentifier,string hubIdentifier, string type , string name, string description ,string location)
        {
            var device = new Device {DeviceIdentifier= deviceIdentifier, Type = type, Name = name, Description = description, Location = location };
            await _deviceDataService.AddDevice(device, hubIdentifier);
        }

        public async Task<List<Device>> GetDevicesFromDataBase()
        {
            return await _deviceDataService.GetDevices();
        }

    }
}
