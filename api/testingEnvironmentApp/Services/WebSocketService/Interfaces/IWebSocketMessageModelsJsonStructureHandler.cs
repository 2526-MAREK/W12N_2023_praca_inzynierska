using System.Net.WebSockets;

namespace testingEnvironmentApp.Services.WebSocketService.Interfaces
{
    public interface IWebSocketMessageModelsJsonStructureHandler
    {
        Task HandleMessageAsync(WebSocket webSocket);
    }
}
