using System.Diagnostics;
using testingEnvironmentApp.Services.Managers.Interfaces;

namespace testingEnvironmentApp.HostedService
{
    public class DeviceManagerBackgroundService : IHostedService
    {
        private readonly IChannelManager _deviceManager;
        private Task _backgroundTask;
        private CancellationTokenSource _cts;

        public DeviceManagerBackgroundService(IChannelManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _backgroundTask = Task.Run(async () =>
            {
                try
                {
                    //while (true)
                    //{
                        await _deviceManager.StartAsync(cancellationToken);
                        // Twoja logika
                    //}
                }
                catch (Exception ex)
                {
                    // Logowanie błędów
                }
            }, _cts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_backgroundTask == null)
            {
                return;
            }

            _cts.Cancel();

            try
            {
                await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
            catch (OperationCanceledException) { }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
