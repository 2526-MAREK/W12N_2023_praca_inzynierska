namespace testingEnvironmentApp.Services.Managers.Interfaces
{
    public interface IChannelManager
    {
        public Task StartAsync(CancellationToken cancellationToken);
        //public Task AddAllDeviceWithUnitPVHistoricalAndAlarmToDataBase();
    }
}
