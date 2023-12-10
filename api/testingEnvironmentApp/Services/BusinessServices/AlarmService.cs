using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    //AlarmService - odpowiada za operacje związane z alarmami, więc również można go zakwalifikować jako serwis biznesowy i umieścić w folderze BusinessServices.
    public class AlarmService : IAlarmService
    {
        IAlarmDataService _alarmDataService;

        public AlarmService(IAlarmDataService alarmDataService)
        {
            _alarmDataService = alarmDataService;
        }

        public async Task<List<Alarm>> GetAllAlarmsFromDataBase()
        {
            return await _alarmDataService.GetAllChannelAlarms();
        }

       public async Task AddAlarmsToDataBase(string channelIdentifier, ICollection<Alarm> alarms)
        {
            foreach (var alarm in alarms)
            {
                await _alarmDataService.AddChannelAlarm(channelIdentifier, alarm);
            }
        }

        public async Task AddAlarmToDataBase(string channelIdentifier, Alarm alarm)
        {
            await _alarmDataService.AddChannelAlarm(channelIdentifier, alarm);
        }

        public async Task FindBadStatusAlarmAndChangeToOkStatusInDataBase(string channelIdentifier, string message1, string message2)
        {
            await _alarmDataService.ResolveAlarms(channelIdentifier, message1, message2);
        }
    }

}
