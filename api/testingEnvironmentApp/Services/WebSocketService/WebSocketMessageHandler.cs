
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.WebSocketService.Interfaces;

namespace testingEnvironmentApp.Services.WebSocketService
{
    public class WebSocketMessageHandler : IWebSocketMessageHandler
    {
        private readonly IMessageQueueValueDeviceForWebSocketDevice _messageQueueValueDeviceForWebSocketDevice;
        private CancellationTokenSource _ctsToProcessAndSendDataAsync;
        private CancellationTokenSource _ctsToReceiveMessagesAsync;

        private readonly object _changeMessageQueueLock = new object();

        public WebSocketMessageHandler(IMessageQueueValueDeviceForWebSocketDevice messageQueueValueDeviceForWebSocketDevice)
        {
            _messageQueueValueDeviceForWebSocketDevice = messageQueueValueDeviceForWebSocketDevice;
            _ctsToProcessAndSendDataAsync = new CancellationTokenSource();
            _ctsToReceiveMessagesAsync = new CancellationTokenSource();
        }

        public void StartTask(WebSocket webSocket)
        {
            // Anuluj poprzednie zadanie, jeśli istnieje
            if (_ctsToProcessAndSendDataAsync != null && !_ctsToProcessAndSendDataAsync.IsCancellationRequested)
            {
                _ctsToProcessAndSendDataAsync.Cancel();
            }

            // Utwórz nowy CancellationTokenSource
            _ctsToProcessAndSendDataAsync = new CancellationTokenSource();

            // Uruchom nowe zadanie z nowym tokenem
            try
            {
                Task.Run(() => ProcessAndSendDataAsync(webSocket, _ctsToProcessAndSendDataAsync.Token));
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("The task is end ProcessAndSendDataAsync from websocket");
            }
        }


        private void ChangeCurrentlySelectedSystem(string nameOfSystem)
        {
            lock (_changeMessageQueueLock)
            {
                _messageQueueValueDeviceForWebSocketDevice.ClearQueue("airSystemPoint_1");
                _messageQueueValueDeviceForWebSocketDevice.ClearQueue("collingSystemPoint_1");
                _messageQueueValueDeviceForWebSocketDevice.ChangeCurrentlySelectedSystem(nameOfSystem);
            }
        }

        public async Task HandleMessageAsync(WebSocket webSocket)
        {
            
            if (webSocket == null)
            {
                throw new ArgumentNullException(nameof(webSocket), "WebSocket nie może być null.");
            }

            Task receiveTask = Task.Run(() => ReceiveMessagesAsync(webSocket));


            try
            {
                await Task.WhenAll(receiveTask);//, processingTask);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Task canceled.");
            }
            finally
            {
                Debug.WriteLine("End Web socket connection with system");
                
                ChangeCurrentlySelectedSystem("nothing");
  
            }



           

        }


        private async Task ReceiveMessagesAsync(WebSocket webSocket)
        {
            bool isOpenConnection = false;
            string receiveMessageFromWebSocket = null;



            while (webSocket.State == WebSocketState.Open)
            {
                var bufferToMessageFromWebSocket = new byte[1024 * 4];
                if (webSocket.State != WebSocketState.Open && !isOpenConnection)
                {
                    Debug.WriteLine("WebSocket connection is not open.");
                    return; // lub break, w zależności od kontekstu
                }
                if (bufferToMessageFromWebSocket == null)
                {
                    // Zaloguj błąd lub ponownie zainicjuj bufor
                    bufferToMessageFromWebSocket = new byte[1024 * 4];
                    Debug.WriteLine("bufferToMessageFromWebSocket: BYŁEM NULLL");
                }

                WebSocketReceiveResult resultFromWebSocket;
                resultFromWebSocket = null;
                try
                {

                    try
                    {
                        resultFromWebSocket = await webSocket.ReceiveAsync(new ArraySegment<byte>(bufferToMessageFromWebSocket), _ctsToReceiveMessagesAsync.Token);
                        if (resultFromWebSocket.MessageType == WebSocketMessageType.Text)
                        {
                            
                            receiveMessageFromWebSocket = Encoding.UTF8.GetString(bufferToMessageFromWebSocket, 0, resultFromWebSocket.Count);
                            Debug.WriteLine(receiveMessageFromWebSocket);
                            if(receiveMessageFromWebSocket == "nothing")
                            {
                                try
                                {
                                    ChangeCurrentlySelectedSystem(receiveMessageFromWebSocket);
                                    bufferToMessageFromWebSocket = new byte[1024 * 4];
                                    _ctsToProcessAndSendDataAsync.Cancel();
                                }
                                catch (TaskCanceledException)
                                {
                                    Debug.WriteLine("End Task ProcessAndSendDataAsync is send");
                                }
                                var buffer = Encoding.UTF8.GetBytes("nothing");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                }
                            }
                            else if(receiveMessageFromWebSocket == "airSystemPoint_1")
                            {
                                ChangeCurrentlySelectedSystem(receiveMessageFromWebSocket);
                                /*bufferToMessageFromWebSocket = new byte[1024 * 4];
                                var buffer = Encoding.UTF8.GetBytes("airSystemPoint_1");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                }*/
                                StartTask(webSocket);
                            }
                            else if(receiveMessageFromWebSocket == "collingSystemPoint_1")
                            {

                                ChangeCurrentlySelectedSystem(receiveMessageFromWebSocket);
                                /*bufferToMessageFromWebSocket = new byte[1024 * 4];
                                var buffer = Encoding.UTF8.GetBytes("collingSystemPoint_1");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                }*/
                                StartTask(webSocket);
                            }
                            else
                            {
                                try
                                {
                                    ChangeCurrentlySelectedSystem("nothing");
                                    bufferToMessageFromWebSocket = new byte[1024 * 4];
                                    _ctsToProcessAndSendDataAsync.Cancel();
                                }
                                catch (TaskCanceledException)
                                {
                                    Debug.WriteLine("End Task ProcessAndSendDataAsync is send");
                                }
                                var buffer = Encoding.UTF8.GetBytes("nothing");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                }
                            }

                            if (receiveMessageFromWebSocket == "close")
                            {
                                Debug.WriteLine("Closing connection with web socket");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Finished communication", CancellationToken.None);
                                }
                                _ctsToProcessAndSendDataAsync.Cancel(); // Anuluj oba zadania
                                _ctsToReceiveMessagesAsync.Cancel();
                                break;
                            }
                            else
                            {
                                isOpenConnection = true;
                                ChangeCurrentlySelectedSystem(receiveMessageFromWebSocket);
                            }
                        }

                    }
                    catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        Debug.WriteLine("WebSocket connection closed prematurely: " + ex.Message);
                        break; // Wyjście z pętli
                    }

                }
                catch (ArgumentException ex)
                {
                    // Obsługa wyjątku ArgumentNullException
                    Debug.WriteLine($"Argument Exception: {ex.Message}");
                    break;
                }

            }
            Debug.WriteLine("RECIEVEMESSAGE Is End");
        }

        private async Task ProcessAndSendDataAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            try
            {
     
                while (!cancellationToken.IsCancellationRequested)
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        if (_messageQueueValueDeviceForWebSocketDevice.GetCurrentlySelectedSystem() != null && _messageQueueValueDeviceForWebSocketDevice.GetCurrentlySelectedSystem() != "nothing")
                        {
                           
  
                            string result = await _messageQueueValueDeviceForWebSocketDevice.DequeueAsync(_messageQueueValueDeviceForWebSocketDevice.GetCurrentlySelectedSystem());
                   
                            var jsonMessage = JsonSerializer.Serialize(result);
                            var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                            if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
                                }
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("ProcessAndSendDataAsync is going end");
                _messageQueueValueDeviceForWebSocketDevice.ClearAllQueues();
                Debug.WriteLine("Debug.WriteLine(_messageQueueValueDeviceForWebSocketDevice.GetMessagesQueue().Count);");
            }

            Debug.WriteLine("----------------------------------------> ProcessAndSendDataAsync Is End <----------------------------------------");
        }



        private async Task SendMessageAsync(WebSocket webSocket, string message1)
        {
            var messages = new { Sensor1 = message1 };
            var jsonMessage = JsonSerializer.Serialize(messages);
            var buffer = Encoding.UTF8.GetBytes(jsonMessage);

            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
