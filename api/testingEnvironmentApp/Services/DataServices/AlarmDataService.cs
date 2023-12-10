using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Models.Alarms.Interfaces;
using testingEnvironmentApp.Models.Devices;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.DataBaseService
{
    public class AlarmDataService : IAlarmDataService
    {
        private readonly ApplicationDbContext _context;

        public AlarmDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ResolveAlarms(string channelIdentifier, string message1, string message2)
        {
            // Znajdź rekordy spełniające kryteria
            var alarmsToUpdate = await _context.Alarms
                .Where(a => a.Status == "Bad" && (a.MessageText == message1 || a.MessageText == message2) && a.ChannelIdentifier == channelIdentifier)
                .ToListAsync();

            if (alarmsToUpdate.Any())
            {
                // Aktualizuj rekordy
                foreach (var alarm in alarmsToUpdate)
                {
                    alarm.Status = "Ok";
                    alarm.ResolutionDate = DateTime.Now;
                }

                // Zapisz zmiany w bazie danych
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Alarm>> GetAllChannelAlarms()
        {
            return await _context.Alarms.ToListAsync();
        }


        public async Task AddChannelAlarm(string channelIdentifier, Alarm alarm)
        {
            _context.Alarms.Add(alarm);
            await _context.SaveChangesAsync();
        }

    }
}
