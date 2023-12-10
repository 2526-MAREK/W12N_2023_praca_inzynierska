using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.WebSocketService.Interfaces;

namespace testingEnvironmentApp.Services.WebSocketService
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly IWebSocketMessageHandler _webSocketMessageHandler;
        private readonly IWebSocketMessageModelsJsonStructureHandler _webSocketMessageModelsJsonStructureHandler;

        public WebSocketHandler(IWebSocketMessageHandler webSocketMessageHandler, IWebSocketMessageModelsJsonStructureHandler webSocketMessageModelsJsonStructureHandler)
        {
            _webSocketMessageHandler = webSocketMessageHandler;
            _webSocketMessageModelsJsonStructureHandler = webSocketMessageModelsJsonStructureHandler;
        }


        public async Task GetDataDisplay(WebSocket webSocket)
        {
            await _webSocketMessageHandler.HandleMessageAsync(webSocket);
            Debug.WriteLine("Zakonczono połączenie z web socket");
        }

        public async Task GetModelsStructureJson(WebSocket webSocket)
        {
            await _webSocketMessageModelsJsonStructureHandler.HandleMessageAsync(webSocket);
        }
    }
}
