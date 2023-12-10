using Microsoft.AspNetCore.Mvc;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.WebSocketService.Interfaces;

namespace testingEnvironmentApp.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketHandler _webSocketHandler;

        public WebSocketController(IWebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [HttpGet("/ws/post")]
        public async Task Post()
        {
            var context = ControllerContext.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                //await _webSocketHandler.HandleAsync(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400; // Bad Request
            }
        }

        [HttpGet("/getData")]
        public async Task GetData()
        {
            var context = ControllerContext.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await _webSocketHandler.GetDataDisplay(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400; // Bad Request
            }
        }

        [HttpGet("/getModelsStructureJson")]
        public async Task GetModelsStructureJson()
        {
            var context = ControllerContext.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await _webSocketHandler.GetModelsStructureJson(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400; // Bad Request
            }
        }
    }
}
