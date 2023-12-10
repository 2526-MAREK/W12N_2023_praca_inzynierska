using System.ComponentModel.DataAnnotations;

namespace testingEnvironmentApp.Models
{
    public class MsrtPoint
    {
        [Key]
        public int IdPoint { get; set; }
        public string MsrtPointIdentifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Factor { get; set; }
        public string Location { get; set; }
    }
}
