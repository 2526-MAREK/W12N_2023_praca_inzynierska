using testingEnvironmentApp.Services.MessageHelper.Model;

namespace testingEnvironmentApp.Services.MessageHelper.Interfaces
{
    public interface ISystemsContainerJsonModel
    {
        public List<ISensorSystemJsonModel> Systems { get; set; }
    }
}
