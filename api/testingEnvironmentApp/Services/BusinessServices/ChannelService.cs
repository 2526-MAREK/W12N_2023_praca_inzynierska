using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelDataService _channelDataService;

        public ChannelService(IChannelDataService channelDataService)
        {
            _channelDataService = channelDataService;
        }


        public async Task AddNewChannelToDevice(string deviceIdentifier, string type, string channelIdentifier, string unit, string factor)
        {
            var channel = new Channel { Type = type, ChannelIdentifier = channelIdentifier, Unit = unit, Factor = factor };
            await _channelDataService.AddChannel(channel, deviceIdentifier);
        }

        public async Task<List<Channel>> GetAllCahnnelsFromDataBase()
        {
            return await _channelDataService.GetAllChannel();
        }

        public async Task<string> GetDeviceIdentifierWithChannelIdentifierFromDataBase(string channelIdentfier)
        {
            return await _channelDataService.GetDeviceIdentifierFromChannel(channelIdentfier);
        }
    }
}
