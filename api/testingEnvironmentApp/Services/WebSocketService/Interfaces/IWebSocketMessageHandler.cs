using System.Net.WebSockets;

namespace testingEnvironmentApp.Services.WebSocketService.Interfaces
{
    public interface IWebSocketMessageHandler
    {
        Task HandleMessageAsync(WebSocket webSocket);
    }
}
