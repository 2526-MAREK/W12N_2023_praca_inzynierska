using System.Diagnostics;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Models.Devices;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    //keyDevice is for Example "airSystem/PreassureSensor_1" = "identifierSystem+"/"+ identifierDevice"
    public interface IChannelFactory
    {
        public void Create(string channelKey, int idChannel, AlarmRange range, bool isAcknowledgedUrgentAlarm, bool isConnectionOk, AlarmStatus actualStatusAlarm, double pV, bool channelIsHistorable);
        public void UpdatePv(string keyDevice, double newPvValue);
        /*public void UpdateAlarms(string keyDevice, ICollection<Alarm> alarms);*/
        public void UpdateActualStatusAlarm(string keyDevice, AlarmStatus alarmStatus);
        public List<Channel> GetAllChannels();
        public List<string> GetAllMsrtPointsAndChannelsIdentifier();
        // Metoda zwracająca urządzenie na podstawie identyfikatora
        public Channel GetChannelByChannelKey(string channelKey);
    }
}
