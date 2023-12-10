using testingEnvironmentApp.Models.Alarms;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IAlarmService
    {
        //Task AddAlarmAsync(string deviceIdentifier, Alarm alarm);
        //Task FindBadStatusAlarmWithMessageProperties(string deviceIdentifier, string message1, string message2);

        public Task<List<Alarm>> GetAllAlarmsFromDataBase();
        public Task AddAlarmToDataBase(string channelIdentifier, Alarm alarm);
        public Task AddAlarmsToDataBase(string channelIdentifier, ICollection<Alarm> alarms);
        public Task FindBadStatusAlarmAndChangeToOkStatusInDataBase(string channelIdentifier, string message1, string message2);
        // Inne metody związane z obsługą alarmów
    }
}
