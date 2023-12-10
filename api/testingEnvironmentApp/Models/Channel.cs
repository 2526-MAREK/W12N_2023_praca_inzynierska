using System.ComponentModel.DataAnnotations.Schema;
using testingEnvironmentApp.Models.Alarms.Interfaces;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Models.Devices.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using testingEnvironmentApp.Models.Interfaces;

namespace testingEnvironmentApp.Models
{
    /// <summary>
    /// Klasa Channel reprezentuje czujnik w systemie, przechowujący informacje o stanie czujnika.
    /// Implementuje interfejsy IHasPv, IAlarmable oraz IHistorable.
    /// </summary>
    public class Channel : IHasPv, IAlarmable, IHistorable
    {
        [Key]
        public int IdChannel { get; set; }
        public int IdDevice { get; set; }
        public string Type { get; set; }
        public string ChannelIdentifier { get; set; }
        public string Unit { get; set; }
        public string Factor { get; set; }
        [NotMapped]
        public AlarmRange Range { get; set; }
        [NotMapped]
        public bool IsConnectionOk { get; set; }
        [NotMapped]
        public double Pv { get; set; }
        [NotMapped]
        public bool IsAcknowledgedUrgentAlarm { get; set; }
        [NotMapped]
        public AlarmStatus ActualStatusAlarm { get; set; }
        [NotMapped]
        public bool ChannelIsHistorable { get; set; }

        /// <summary>
        /// Sprawdza, czy warunki alarmowe są spełnione i generuje alarmy.
        /// </summary>
        /// <returns>Zwraca listę wygenerowanych alarmów.</returns>
        public IEnumerable<Alarm> CheckAlarms()
        {
            List<Alarm> generatedAlarms = new List<Alarm>();

            bool isHighAlarm = Range.UrgentHiHi != null && Range.WarningHi != null && Pv >= Range.WarningHi && Pv <= Range.UrgentHiHi;
            bool isLowAlarm = Range.UrgentLoLo != null && Range.WarningLo != null && Pv >= Range.UrgentLoLo && Pv <= Range.WarningLo;
            bool isWarningAlarm = isHighAlarm || isLowAlarm;

            bool isHighHighAlarm = Range.UrgentHiHi != null && Pv >= Range.UrgentHiHi;
            bool isLowLowAlarm = Range.UrgentLoLo != null && Pv <= Range.UrgentLoLo;
            bool isUrgentAlarm = isHighHighAlarm || isLowLowAlarm;

            if (isUrgentAlarm && IsAcknowledgedUrgentAlarm)
            {
                string message;
                string possibleFault;

                
                if (isHighHighAlarm)
                {
                    message = "Urgent High Alarm";
                    possibleFault = $"Device exceeds high high limit. Pv = {Pv}. Check Device is must be problem";
                    generatedAlarms.Add(new Alarm
                    {
                        IdChannel = IdChannel,
                        ChannelIdentifier = ChannelIdentifier,
                        OccurrenceDate = DateTime.Now,
                        Status = "Bad",
                        MessageText = $"{message}",
                        PossibleFault = $"{possibleFault}"
                    });
                    //musimy wysłac sygnał do frontend 
                    IsAcknowledgedUrgentAlarm = false;
                    ActualStatusAlarm = AlarmStatus.Urgency;
                }


                if (isLowLowAlarm)
                {
                    message = "Urgent Low Alarm";
                    possibleFault = $"Device exceeds low low limit. Pv = {Pv}. Check Device is must be problem";
                    generatedAlarms.Add(new Alarm
                    {
                        IdChannel = IdChannel,
                        ChannelIdentifier = ChannelIdentifier,
                        OccurrenceDate = DateTime.Now,
                        Status = "Bad",
                        MessageText = $"{message}",
                        PossibleFault = $"{possibleFault}"
                    });
                    //musimy wysłac sygnał do frontend 
                    IsAcknowledgedUrgentAlarm = false;
                    ActualStatusAlarm = AlarmStatus.Urgency;
                }

            }


            if (ActualStatusAlarm != AlarmStatus.Urgency)
            {
                if (ActualStatusAlarm != AlarmStatus.Warning)
                {

                    string message;
                    string possibleFault;


                    if (isHighAlarm)
                    {
                        message = "Warning High Alarm";
                        possibleFault = $"Device exceeds high limit. Pv = {Pv}. Check Device is must be problem";
                        generatedAlarms.Add(new Alarm
                        {
                            IdChannel = IdChannel,
                            ChannelIdentifier = ChannelIdentifier,
                            OccurrenceDate = DateTime.Now,
                            Status = "Bad",
                            MessageText = $"{message}",
                            PossibleFault = $"{possibleFault}"
                        });
                        ActualStatusAlarm = AlarmStatus.Warning;
                    }

                    if (isLowAlarm)
                    {
                        message = "Warning Low Alarm";
                        possibleFault = $"Device exceeds low limit. Pv = {Pv}. Check Device is must be problem";
                        generatedAlarms.Add(new Alarm
                        {
                            IdChannel = IdChannel,
                            ChannelIdentifier = ChannelIdentifier,
                            OccurrenceDate = DateTime.Now,
                            Status = "Bad",
                            MessageText = $"{message}",
                            PossibleFault = $"{possibleFault}"
                        });
                        ActualStatusAlarm = AlarmStatus.Warning;
                    }

                }


            }
            Console.WriteLine(ActualStatusAlarm);


            return generatedAlarms;
        }
    }
}
