using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Channels;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.DataServices.Interfaces;
using Channel = testingEnvironmentApp.Models.Channel;

namespace testingEnvironmentApp.Services.DataServices
{
    public class ChannelDataService : IChannelDataService
    {
        private readonly ApplicationDbContext _context;

        public ChannelDataService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddChannel(testingEnvironmentApp.Models.Channel newChannel, string deviceIdentifier)
        {



            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            // Sprawdzenie, czy urządzenie już istnieje
            var existingChannel = await _context.Channels
                                                .FirstOrDefaultAsync(d => d.ChannelIdentifier == newChannel.ChannelIdentifier);

            if (existingChannel != null)
            {
                Debug.WriteLine($"Error: Device with identifier {existingChannel.ChannelIdentifier} already exists.");
                return;
            }

            try
            {
                var idDevice = await _context.Devices
            .Where(d => d.DeviceIdentifier == deviceIdentifier)
            .Select(d => d.IdDevice)
            .FirstOrDefaultAsync();

                if (idDevice != null)
                {
                    newChannel.IdDevice = idDevice;
                    _context.Channels.Add(newChannel);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Debug.WriteLine("This id Hub dont exists");
                }
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Failed to add device: {ex.Message}. InnerException: {ex.InnerException?.Message}");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine($"Object disposed error occurred: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }

        public async Task<List<Channel>> GetAllChannel()
        {
            return await _context.Channels.ToListAsync();
        }

        public async Task<string> GetDeviceIdentifierFromChannel(string channelIdentifier)
        {
            var idDevice = await _context.Channels
          .Where(d => d.ChannelIdentifier == channelIdentifier)
          .Select(d => d.IdDevice)
          .FirstOrDefaultAsync();

            var deviceIdentifier = await _context.Devices
         .Where(d => d.IdDevice == idDevice)
         .Select(d => d.DeviceIdentifier)
         .FirstOrDefaultAsync();

            if (deviceIdentifier != null)
            {
                return deviceIdentifier;
            }
            else
            {
                return null;
            }
        }
    }
}
