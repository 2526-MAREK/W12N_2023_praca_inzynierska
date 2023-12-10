using testingEnvironmentApp.Models;
using testingEnvironmentApp.Data;
using System.Diagnostics;
using testingEnvironmentApp.Services.DataServices.Interfaces;
using testingEnvironmentApp.Models.Devices;
using Device = testingEnvironmentApp.Models.Device;
using Microsoft.EntityFrameworkCore;

namespace testingEnvironmentApp.Services.DataBaseService
{
    public class DeviceDataService : IDeviceDataService
    {
        private readonly ApplicationDbContext _context;

        public DeviceDataService(ApplicationDbContext context)
        {
            _context = context;
            Debug.WriteLine("DeviceDataService constructed.");
        }

        public async Task AddDevice(Device newDevice, string hubIdentifier)
        {
            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            // Sprawdzenie, czy urządzenie już istnieje
            var existingDevice = await _context.Devices
                                                .FirstOrDefaultAsync(d => d.DeviceIdentifier == newDevice.DeviceIdentifier);

            if (existingDevice != null)
            {
                Debug.WriteLine($"Error: Device with identifier {existingDevice.DeviceIdentifier} already exists.");
                return;
            }


            var idHub = await _context.Hubs
            .Where(d => d.HubIdentifier == hubIdentifier)
            .Select(d => d.IdHub)
            .FirstOrDefaultAsync();

            newDevice.IdHub = idHub;

            _context.Devices.Add(newDevice);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Device>> GetDevices()
        {
            return await _context.Devices .ToListAsync();
        }

    }
}
