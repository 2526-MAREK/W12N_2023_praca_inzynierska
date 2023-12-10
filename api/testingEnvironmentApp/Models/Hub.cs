using System.ComponentModel.DataAnnotations;

namespace testingEnvironmentApp.Models
{
    public class Hub
    {
        [Key]
        public int IdHub { get; set; }
        public string HubIdentifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
