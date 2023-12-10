using testingEnvironmentApp.Services.MessageHelper.Interfaces;

namespace testingEnvironmentApp.Services.MessageHelper.Model
{
    public class SensorSystemJsonModel : ISensorSystemJsonModel
    {
        public string System { get; set; }
        public List<SensorJsonModel> Sensors { get; set; }
    }
}
