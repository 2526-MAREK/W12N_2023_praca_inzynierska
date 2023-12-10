using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testingEnvironmentApp.Models.Alarms
{
    public class Alarm
    {
        [Key]
        public int IdAlarm { get; set; }
        public int IdChannel { get; set; }
        public string ChannelIdentifier { get; set; }
        public DateTime OccurrenceDate { get; set; }
        public string Status { get; set; }
        public string PossibleFault { get; set; }
        public string MessageText { get; set; }

        public DateTime ResolutionDate { get; internal set; }
    }
}
