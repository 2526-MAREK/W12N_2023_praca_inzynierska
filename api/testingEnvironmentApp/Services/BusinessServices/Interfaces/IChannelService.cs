using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IChannelService
    {
        public Task AddNewChannelToDevice(string deviceIdentifier, string type, string channelIdentifier, string unit, string factor);
        public Task<List<Channel>> GetAllCahnnelsFromDataBase();
        public Task<string> GetDeviceIdentifierWithChannelIdentifierFromDataBase(string channelIdentfier);
    }
}
