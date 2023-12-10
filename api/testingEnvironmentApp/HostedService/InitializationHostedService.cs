using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.Implementations.Interfaces;

namespace testingEnvironmentApp.HostedService
{
    public class InitializationHostedService : IHostedService
    {
        private readonly IObjectsInitializer _initializer;
        private readonly IServiceScopeFactory _scopeFactory;

        public InitializationHostedService(IObjectsInitializer initializer, IServiceScopeFactory scopeFactory)
        {
            _initializer = initializer;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var initializerDataBase = scope.ServiceProvider.GetService<IObjectsToDataBaseInitializer>();
                //Debug.WriteLine("Halo1GRRRRRRRRRRR");
                await initializerDataBase.InitializeObjectsToDataBase();
            }
            //Debug.WriteLine("Halo2GRRRRRRRRRRR");
            await _initializer.InitializeObjectsAsync();
            _initializer.InitializeObjects();
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up any resources if needed
            return Task.CompletedTask;
        }
        // ... StopAsync itd.
    }
}
