

using testingEnvironmentApp.Services.BusinessServices.ModelForJsonFactory;

namespace testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory
{
    public interface ISystemsContainerJsonModel
    {
        public List<SensorSystemJsonModel> Systems { get; set; }
    }
}
