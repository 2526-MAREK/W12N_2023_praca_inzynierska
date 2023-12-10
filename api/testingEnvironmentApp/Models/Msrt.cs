using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testingEnvironmentApp.Models
{
    public class Msrt
    {
       // [Key]
        //public int IdMsrt { get; set; } // Unikalny identyfikator jako klucz główny
        public int IdHubs { get; set; }
        public int IdDevice { get; set; }
        public int IdChannels { get; set; }
        public double MsrtValue { get; set; }
        
        public DateTime DataTimeMs { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string DateTimeZone { get; set; }
    }
}
