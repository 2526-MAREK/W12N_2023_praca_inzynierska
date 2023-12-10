using System.Net.WebSockets;
using System.Net;

namespace testingEnvironmentApp.Middleware
{
    public class WebSocketRequestHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public WebSocketRequestHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleRequestAsync(HttpContext context, Func<WebSocket, Task> specificHandler)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await specificHandler(webSocket);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }
    }
}
