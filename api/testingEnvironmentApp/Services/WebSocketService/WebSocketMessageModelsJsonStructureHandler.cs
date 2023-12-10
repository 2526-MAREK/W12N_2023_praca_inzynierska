using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using testingEnvironmentApp.Services.Managers;
using testingEnvironmentApp.Services.Managers.Interfaces;
using testingEnvironmentApp.Services.MessageQueueService;
using testingEnvironmentApp.Services.MessageQueueService.Interfaces;
using testingEnvironmentApp.Services.WebSocketService.Interfaces;

namespace testingEnvironmentApp.Services.WebSocketService
{
    public class WebSocketMessageModelsJsonStructureHandler : IWebSocketMessageModelsJsonStructureHandler
    {

        private readonly IMessageQueueForModelsStructureJson _messageQueueForModelsStructureJson;
        private readonly IModelsStructureJsonManager _modelsStructureJsonManager;

        private CancellationTokenSource _ctsToProcessAndSendDataAsync;
        private CancellationTokenSource _ctsToReceiveMessagesAsync;

        private readonly object _changeMessageQueueLock = new object();

        public WebSocketMessageModelsJsonStructureHandler(IMessageQueueForModelsStructureJson messageQueueForModelsStructureJson, IModelsStructureJsonManager modelsStructureJsonManager)
        {
            _messageQueueForModelsStructureJson = messageQueueForModelsStructureJson;
            _modelsStructureJsonManager = modelsStructureJsonManager;

            _ctsToProcessAndSendDataAsync = new CancellationTokenSource();
            _ctsToReceiveMessagesAsync = new CancellationTokenSource();
        }

        private void ChangeCurrentlySelectedOptions(string nameOfOptions)
        {
            lock (_changeMessageQueueLock)
            {
                _messageQueueForModelsStructureJson.ClearQueue("normalFlowStructureAllObjectWithBasicInfoJson");
                _messageQueueForModelsStructureJson.ClearQueue("normalFlowStructureAllObjectToFastModifyJson");
                _messageQueueForModelsStructureJson.ChangeCurrentlySelectedOptions(nameOfOptions);
            }
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

            }

        }


        private async Task ReceiveMessagesAsync(WebSocket webSocket)
        {
            bool isOpenConnection = false;
            string receiveMessageFromWebSocket = null;

            bool isInitializeStructureAllObjectWithBasicInfoJson = false;
            bool isInitializeStructureAllObjectToFastModifyJson = false;



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
                            if (receiveMessageFromWebSocket == "initializeStructureAllObjectWithBasicInfoJson" && !isInitializeStructureAllObjectWithBasicInfoJson)
                            {
                                try
                                {

                                   


                                    var jsonMessage = JsonSerializer.Serialize(_modelsStructureJsonManager.InitializeStructureAllObjectWithBasicInfoJson());
                                    var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                                    if (webSocket.State == WebSocketState.Open)
                                    {
                                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                    }
                                    bufferToMessageFromWebSocket = new byte[1024 * 4];

                                    Debug.WriteLine("Send initializeStructureAllObjectWithBasicInfoJson ");
                                    isInitializeStructureAllObjectWithBasicInfoJson = true;
                                    ChangeCurrentlySelectedOptions("initializeStructureAllObjectToFastModifyJson");
                                }
                                catch (TaskCanceledException)
                                {
                                    Debug.WriteLine("End Task ProcessAndSendDataAsync is send");
                                }



                            }
                            else if (receiveMessageFromWebSocket == "initializeStructureAllObjectToFastModifyJson" && !isInitializeStructureAllObjectToFastModifyJson )
                            {
                                try
                                {


                                    var jsonMessage = JsonSerializer.Serialize(_modelsStructureJsonManager.InitializeStructureAllObjectToFastModifyJson());
                                    var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                                    if (webSocket.State == WebSocketState.Open)
                                    {
                                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                    }
                                    bufferToMessageFromWebSocket = new byte[1024 * 4];
                                    Debug.WriteLine("Send initializeStructureAllObjectToFastModifyJson ");
                                    isInitializeStructureAllObjectToFastModifyJson = true;
                                    ChangeCurrentlySelectedOptions("normalFlowStructureAllObjectToFastModifyJson");
                                }
                                catch (TaskCanceledException)
                                {
                                    Debug.WriteLine("End Task ProcessAndSendDataAsync is send");
                                }
                            }
                            else if (receiveMessageFromWebSocket == "normalFlowStructureAllObjectToFastModifyJson")
                            {
                                Debug.WriteLine(" I am in normal flow");
                                ChangeCurrentlySelectedOptions("normalFlowStructureAllObjectToFastModifyJson");

                                bufferToMessageFromWebSocket = new byte[1024 * 4];
                                var buffer = Encoding.UTF8.GetBytes("normalFlowStructureAllObjectToFastModifyJson");
                                if (webSocket.State == WebSocketState.Open)
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _ctsToReceiveMessagesAsync.Token);
                                }
                                StartTask(webSocket);
                            }
                            else
                            {
                                try
                                {
                                    bufferToMessageFromWebSocket = new byte[1024 * 4];
                                    _ctsToProcessAndSendDataAsync.Cancel();
                                }
                                catch (TaskCanceledException)
                                {
                                    Debug.WriteLine("End Task ProcessAndSendDataAsync is send");
                                }
                                var buffer = Encoding.UTF8.GetBytes("endNormalFlow");
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
                                //ChangeCurrentlySelectedOptions("nothing");
                            }
                        }

                        // Reszta logiki...
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
                Debug.WriteLine("Halo1");
                while (!cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Halo2");
                    while (webSocket.State == WebSocketState.Open)
                    {
                        Debug.WriteLine("Halo3");
                        Debug.WriteLine(_messageQueueForModelsStructureJson.GetCurrentlySelectedOptions());
                        if (_messageQueueForModelsStructureJson.GetCurrentlySelectedOptions() != null && _messageQueueForModelsStructureJson.GetCurrentlySelectedOptions() != "nothing")
                        {
                            Debug.WriteLine("Halo4");
                            if (_messageQueueForModelsStructureJson.GetCurrentlySelectedOptions() == "normalFlowStructureAllObjectToFastModifyJson")
                            {
                                Debug.WriteLine("Czekam w kolejce....");
                                string result = await _messageQueueForModelsStructureJson.DequeueAsync(_messageQueueForModelsStructureJson.GetCurrentlySelectedOptions());
                                

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
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("ProcessAndSendDataAsync is going end");
                _messageQueueForModelsStructureJson.ClearAllQueues();
                
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
