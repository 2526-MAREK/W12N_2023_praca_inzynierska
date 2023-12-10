using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory
{
    public class SensorSystemJsonModel : InterfacesForJsonFactory.ISensorSystemJsonModel
    {
        public string System { get; set; }
        public List<SensorJsonModel> Sensors { get; set; }
    }
}
