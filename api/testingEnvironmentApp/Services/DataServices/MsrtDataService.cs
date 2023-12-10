using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Devices;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.DataServices
{
    public class MsrtDataService : IMsrtDataService
    {
        private readonly ApplicationDbContext _context;

        public MsrtDataService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddMsrt(Msrt newMsrt)
        {
            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            _context.Msrts.Add(newMsrt);
            await _context.SaveChangesAsync();
        }

        public async Task AddMeasurement(string channelIdentifier, double value)
        {
            // Znajdź Channel na podstawie ChannelIdentifier
            var channel = await _context.Channels
                .FirstOrDefaultAsync(c => c.ChannelIdentifier == channelIdentifier);

            if (channel == null)
            {
                throw new ArgumentException("Channel not found");
            }
            var idDevice = channel.IdDevice;

            var idHub = await _context.Devices
            .Where(d => d.IdDevice == idDevice)
            .Select(d => d.IdHub)
            .FirstOrDefaultAsync();



            // Utwórz nowy pomiar (Msrt)
            var newMeasurement = new Msrt
            {
                IdHubs = idHub,
                IdDevice = idDevice,
                IdChannels = channel.IdChannel,
                MsrtValue = value,
                DataTimeMs = DateTime.UtcNow, // Załóżmy, że używamy UTC
                DateTimeZone = TimeZoneInfo.Utc.Id, // Możesz ustawić strefę czasową według potrzeb
            };

            


            /* var existingMsrt = await _context.Msrts
      .FindAsync(newMeasurement.IdHubs, newMeasurement.IdDevice, newMeasurement.IdChannels, newMeasurement.DataTimeMs, newMeasurement.DateTimeZone);

             if (existingMsrt == null)
             {*/
            // Rekord jest unikalny, możemy go dodać
            _context.Msrts.Add(newMeasurement);
                await _context.SaveChangesAsync();
            /*}
            else
            {
                Debug.WriteLine("This msrt is exisiting in database");
            }*/
        }

        public async Task<List<Msrt>> GetMeasurementsByChannelAndDateRange(string channelIdentifier, DateTime startDate, DateTime endDate)
        {
            // Znajdź Channel na podstawie ChannelIdentifier
            var channel = await _context.Channels
                .FirstOrDefaultAsync(c => c.ChannelIdentifier == channelIdentifier);

            if (channel == null)
            {
                // Zwróć pustą listę, jeśli nie znaleziono kanału
                return new List<Msrt>();
            }

            // Pobierz pomiary dla danego kanału i zakresu dat
            var measurements = await _context.Msrts
                .Where(m => m.IdChannels == channel.IdChannel &&
                            m.DataTimeMs >= startDate &&
                            m.DataTimeMs <= endDate)
                .ToListAsync();

            // Zwróć pustą listę, jeśli nie znaleziono pomiarów
            return measurements ?? new List<Msrt>();
        }

    }
}
