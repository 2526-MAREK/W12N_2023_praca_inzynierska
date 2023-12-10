using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class MsrtService : IMsrtService
    {

        IMsrtDataService _channelDataService;
        public MsrtService(IMsrtDataService channelDataService)
        {
            _channelDataService = channelDataService;
        }

        public async Task CreateNewMsrtToChannel(string channelIdentifier, double value)
        {
            await _channelDataService.AddMeasurement(channelIdentifier, value);
        }


        public async Task<List<Msrt>> GetMeasurementsByChannelAndDateRangeFromDataBase(string channelIdentifier, DateTime startDate, DateTime endDate)
        {
            return await _channelDataService.GetMeasurementsByChannelAndDateRange(channelIdentifier, startDate, endDate);
        }
    }
}
