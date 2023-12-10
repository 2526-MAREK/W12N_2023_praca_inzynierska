using System.Net.WebSockets;

namespace testingEnvironmentApp.Services.WebSocketService.Interfaces
{
    public interface IWebSocketHandler
    {
        Task GetDataDisplay(WebSocket webSocket);
        public Task GetModelsStructureJson(WebSocket webSocket);
    }
}
