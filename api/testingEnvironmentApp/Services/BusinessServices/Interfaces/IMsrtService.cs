using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IMsrtService
    {
        public Task CreateNewMsrtToChannel(string channelIdentifier, double value);
        public Task<List<Msrt>> GetMeasurementsByChannelAndDateRangeFromDataBase(string channelIdentifier, DateTime startDate, DateTime endDate);
    }
}
