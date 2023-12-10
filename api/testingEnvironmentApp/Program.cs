using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using testingEnvironmentApp.Services.WebSocketService;
using testingEnvironmentApp.Services.MqttService;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Services.DataBaseService;
using testingEnvironmentApp.Services.WebSocketService.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.BusinessServices;
using testingEnvironmentApp.Services.ManagerService;
using testingEnvironmentApp.Services.Managers.Interfaces;
using testingEnvironmentApp.Services.MqttService.Interfaces;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices.Interfaces;
using testingEnvironmentApp.Services.PublicSubscribeService.Interfaces;
using testingEnvironmentApp.Services.PublicSubscribeService;
using testingEnvironmentApp.HostedService;
using testingEnvironmentApp.Services.Implementations.Interfaces;
using testingEnvironmentApp.Services.Implementations;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.BusinessServices.InterfacesForJsonFactory;
using testingEnvironmentApp.Services.DataServices;
using testingEnvironmentApp.Services.Managers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("testingEnvironmentAppDBCon")));


builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddSingleton<IChannelFactory, ChannelFactory>();
builder.Services.AddSingleton<IDeviceValueJsonFactory, DeviceValueJsonFactory>();
builder.Services.AddSingleton<IModelsStructureJsonFactory, ModelsStructureJsonFactory>();
builder.Services.AddSingleton<IObjectsInitializer, ObjectsInitializer>();


builder.Services.AddSingleton<InitializationHostedService>();
builder.Services.AddHostedService<InitializationHostedService>();



builder.Services.AddSingleton<IMqttService, MqttService>(serviceProvider =>
{
    var deviceInitializer = serviceProvider.GetRequiredService<IObjectsInitializer>();
    if (!deviceInitializer.IsInitializationComplete())
    {
        throw new InvalidOperationException("Urz¹dzenia musz¹ byæ zainicjalizowane przed uruchomieniem MqttService.");
    }
    var deviceFactory = serviceProvider.GetRequiredService<IChannelFactory>();
    var deviceValueJsonFactory = serviceProvider.GetRequiredService<IDeviceValueJsonFactory>();
    var messSerivce = serviceProvider.GetRequiredService<IMessageQueueValueDeviceForWebSocketDevice>();
    var messageQueueValueDeviceForDeviceManager = serviceProvider.GetRequiredService<IMessageQueueValueDeviceForDeviceManager>();
    var messageQueueForMsrtSaveToDataBase = serviceProvider.GetRequiredService < IMessageQueueForMsrtSaveToDataBase>();

    return new MqttService(messSerivce, messageQueueValueDeviceForDeviceManager, deviceFactory, deviceValueJsonFactory, messageQueueForMsrtSaveToDataBase);
});


builder.Services.AddScoped<IDeviceDataService, DeviceDataService>();
builder.Services.AddScoped<IAlarmDataService, AlarmDataService>();
builder.Services.AddScoped<IChannelDataService, ChannelDataService>();
builder.Services.AddScoped<IHubDataService, HubDataService>();
builder.Services.AddScoped<IMsrtAssociationDataService, MsrtAssociationDataService>();
builder.Services.AddScoped<IMsrtDataService, MsrtDataService>();
builder.Services.AddScoped<IMsrtPointDataService, MsrtPointDataService>();


builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IAlarmService, AlarmService>();
builder.Services.AddTransient<IChannelService, ChannelService>();
builder.Services.AddTransient<IHubService, HubService>();
builder.Services.AddTransient<IMsrtAssociationService, MsrtAssociationService>();
builder.Services.AddTransient<IMsrtPointService, MsrtPointService>();
builder.Services.AddTransient<IMsrtService, MsrtService>();

builder.Services.AddTransient<IObjectsToDataBaseInitializer, ObjectsToDataBaseInitializer>();

builder.Services.AddSingleton<IChannelManager, ChannelManager>();
builder.Services.AddSingleton <IModelsStructureJsonManager, ModelsStructureJsonManager>();


builder.Services.AddHostedService<DeviceManagerBackgroundService>();

builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
builder.Services.AddSingleton<IWebSocketMessageHandler, WebSocketMessageHandler>();
builder.Services.AddSingleton < IWebSocketMessageModelsJsonStructureHandler, WebSocketMessageModelsJsonStructureHandler>();

builder.Services.AddSingleton<IMessageQueueValueDeviceForWebSocketDevice, MessageQueueValueDeviceForWebSocketDevice>();
builder.Services.AddSingleton<IMessageQueueValueDeviceForDeviceManager, MessageQueueValueDeviceForDeviceManager>();
builder.Services.AddSingleton<IMessageQueueForModelsStructureJson, MessageQueueForModelsStructureJson>();
builder.Services.AddSingleton < IMessageQueueForMsrtSaveToDataBase, MessageQueueForMsrtSaveToDataBase>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWebSockets();
app.UseRouting();

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    logger.LogInformation("Rozpoczynanie inicjalizacji serwisu XYZ...");
    var initService = app.Services.GetRequiredService<InitializationHostedService>();
await initService.StartAsync(new CancellationToken());
    logger.LogInformation("Inicjalizacja serwisu XYZ zakoñczona sukcesem.");
    var mqttService = app.Services.GetRequiredService<IMqttService>() as MqttService;
await mqttService.StartAsync(new CancellationToken());
    logger.LogInformation("Inicjalizacja serwisu XYZ zakoñczona sukcesem.");


    logger.LogInformation("Inicjalizacja serwisu XYZ zakoñczona sukcesem.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Wyst¹pi³ b³¹d podczas inicjalizacji serwisu XYZ.");
}
app.Run();
