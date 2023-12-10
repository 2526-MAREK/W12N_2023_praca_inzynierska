using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.DataServices
{
    public class HubDataService : IHubDataService
    {
        private readonly ApplicationDbContext _context;


        public HubDataService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddHub(Hub newHub)
        {
            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            // Sprawdzenie, czy urządzenie już istnieje
            var existingHub = await _context.Hubs
                                                .FirstOrDefaultAsync(d => d.HubIdentifier == newHub.HubIdentifier);

            if (existingHub != null)
            {
                Debug.WriteLine($"Error: Device with identifier {existingHub.HubIdentifier} already exists.");
                return;
            }


            _context.Hubs.Add(newHub);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetHubIdentifierWithDeviceIdentifier(string deviceIdentifier)
        {
            var idHub = await _context.Devices
         .Where(d => d.DeviceIdentifier == deviceIdentifier)
         .Select(d => d.IdHub)
         .FirstOrDefaultAsync();

            var hubIdentifier = await _context.Hubs
          .Where(d => d.IdHub == idHub)
          .Select(d => d.HubIdentifier)
          .FirstOrDefaultAsync();

            if (hubIdentifier != null)
            {
                return hubIdentifier;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Hub>> GetAllHubs()
        {
            return await _context.Hubs.ToListAsync();
        }
    }
}
