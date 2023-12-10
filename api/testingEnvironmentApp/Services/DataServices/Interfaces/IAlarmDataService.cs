using testingEnvironmentApp.Models.Alarms;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IAlarmDataService
    {
        public Task ResolveAlarms(string channelIdentifier, string message1, string message2);
        public Task<List<Alarm>> GetAllChannelAlarms();

        public Task AddChannelAlarm(string channelIdentifier, Alarm alarm);
    }
}
