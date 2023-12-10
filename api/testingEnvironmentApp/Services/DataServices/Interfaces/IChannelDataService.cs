using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IChannelDataService
    {
        public Task AddChannel(Channel newChannel, string deviceIdentifier);
        public  Task<List<Channel>> GetAllChannel();
        public Task<string> GetDeviceIdentifierFromChannel(string channelIdentifier);
    }
}
