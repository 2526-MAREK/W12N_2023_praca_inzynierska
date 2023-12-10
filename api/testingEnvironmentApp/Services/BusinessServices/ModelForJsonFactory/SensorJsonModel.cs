using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory
{
    public class SensorJsonModel : InterfacesForJsonFactory.ISensorJsonModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
