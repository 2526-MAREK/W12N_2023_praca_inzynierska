

using testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory
{
    public interface ISensorSystemJsonModel
    {
        public string System { get; set; }
        public List<SensorJsonModel> Sensors { get; set; }
    }
}
