using testingEnvironmentApp.Services.MessageHelper.Model;

namespace testingEnvironmentApp.Services.MessageHelper.Interfaces
{
    public interface ISensorSystemJsonModel
    {
        public string System { get; set; }
        public List<SensorJsonModel> Sensors { get; set; }
    }
}
