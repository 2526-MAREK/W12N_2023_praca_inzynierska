using System.ComponentModel.DataAnnotations;

namespace testingEnvironmentApp.Models
{
    public class Device
    {
        [Key]
        public int IdDevice { get; set; }
        public int IdHub { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
