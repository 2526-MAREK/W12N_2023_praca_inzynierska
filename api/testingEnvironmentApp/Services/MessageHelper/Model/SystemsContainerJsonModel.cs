using testingEnvironmentApp.Services.MessageHelper.Interfaces;

namespace testingEnvironmentApp.Services.MessageHelper.Model
{
    public class SystemsContainerJsonModel : ISystemsContainerJsonModel
    {
        public List<ISensorSystemJsonModel> Systems { get; set; }
    }
}
