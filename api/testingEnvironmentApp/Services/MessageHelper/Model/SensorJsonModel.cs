using testingEnvironmentApp.Services.MessageHelper.Interfaces;

namespace testingEnvironmentApp.Services.MessageHelper.Model
{
    public class SensorJsonModel : ISensorJsonModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
