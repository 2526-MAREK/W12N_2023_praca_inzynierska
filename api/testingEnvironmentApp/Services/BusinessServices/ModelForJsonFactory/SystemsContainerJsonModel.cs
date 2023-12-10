using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory
{
    public class SystemsContainerJsonModel : ISystemsContainerJsonModel
    {
        public List<SensorSystemJsonModel> Systems { get; set; }
    }
}
