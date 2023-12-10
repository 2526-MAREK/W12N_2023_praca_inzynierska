namespace testingEnvironmentApp.HostedService.Interfaces
{
    public interface IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken);
    }
}
