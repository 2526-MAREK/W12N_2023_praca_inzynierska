using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IMsrtDataService
    {
        public Task AddMsrt(Msrt newMsrt);
        public Task AddMeasurement(string channelIdentifier, double value);
        public Task<List<Msrt>> GetMeasurementsByChannelAndDateRange(string channelIdentifier, DateTime startDate, DateTime endDate);
    }
}
