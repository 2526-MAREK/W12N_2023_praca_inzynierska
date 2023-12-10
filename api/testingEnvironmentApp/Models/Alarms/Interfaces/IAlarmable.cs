using System.ComponentModel.DataAnnotations.Schema;

namespace testingEnvironmentApp.Models.Alarms.Interfaces
{
    /// <summary>
    /// Interfejs IAlarmable definiuje funkcjonalność potrzebną do obsługi alarmów.
    /// </summary>
    public interface IAlarmable
    {
        [NotMapped]
        public AlarmRange Range { get; set; }
        [NotMapped]
        public double Pv { get; set; }
        [NotMapped]
        public bool IsAcknowledgedUrgentAlarm { get; set; }
        [NotMapped]
        public AlarmStatus ActualStatusAlarm { get; set; }

        /// <summary>
        /// Sprawdza, czy wartości czujnika przekraczają progi alarmowe i generuje nowe alarmy.
        /// </summary>
        /// <returns>Zwraca kolekcję alarmów.</returns>
        IEnumerable<Alarm> CheckAlarms();
    }
}
