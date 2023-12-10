using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testingEnvironmentApp.Models
{
    public class MsrtAssociation
    {
        [Key]
        public int IdAssociation { get; set; }
        //public int IdMsrt { get; set; } // Klucz obcy odnoszący się do Msrt   
        public int IdPoint { get; set; }
        public int IdHubs { get; set; }
        public int IdDevice { get; set; }
        public int IdChannels { get; set; }
        public DateTime DataTimeMs { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string DateTimeZone { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
