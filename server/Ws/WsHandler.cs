using System.Collections.Concurrent;
using Fleck;
using Newtonsoft.Json;
using LogLevel = Fleck.LogLevel;

namespace GameInv.Ws {
    /// <inheritdoc />
    public class WsHandler : IConnectionHandler {
        private static readonly Logger Log = GetLogger();
        private static readonly ConcurrentDictionary<Guid, IWebSocketConnection> AllSockets = new();
        private readonly AutoResetEvent _sleepUntilStopped = new(false);

        private GameInv _gameInv = null!;
        private WebSocketServer _server = null!;
        /// <remarks>
        ///     Only call this once per instance
        /// </remarks>
        public void Start() {
            if (_gameInv == null!) {
                throw new InvalidOperationException("GameInv not set");
            }
            
            
            /*
             *
             * TODO: auth - don't send to socket if not authed
             * TODO: add a method that bypasses auth - for messages pre-auth
             * TODO: extend the socket class - and call base methods that get hidden
             * TODO: make an authenticated property with setter and getter
             * 
             * TODO: READ ME - nvm, actually creste a wrapper
             * TOSO: store the instance of the wrapper indide of start scope, the wrapoer contains a peivate ref to the real socket (using interface), passed using constructor
             * _socket to register events, other stuff using rhe erapped socket
             * https://github.com/statianzo/Fleck/issues/201#issuecomment-313807486
             */

            _gameInv.Inventory.ItemsChanged += SendItems;

            const string authenticated = "authenticated"; // TODO: move this to ^

            _server = new(WsUri);
            FleckLog.LogAction = (_, _, _) => { }; // Disable FleckLog logging
            _server.Start(socket => {
                socket.OnOpen = () => {
                    if (!AllSockets.TryAdd(socket.ConnectionInfo.Id, socket)) return;
                    Log.Info($"Socket {socket.ConnectionInfo.Id} connected");

                    // Store auth state
                    socket.ConnectionInfo.Headers[authenticated] = "false";

                    // Set a timeout to disconnect if no auth received
                    Task.Run(async () => {
                        await Task.Delay(5000);
                        if (!socket.IsAvailable || socket.ConnectionInfo.Headers[authenticated] == "true") return;
                        await socket.Send(EncodeMessage("disconnect", null, "Failed auth"));
                        socket.Close();
                        Log.Info($"Socket {socket.ConnectionInfo.Id} disconnected due to auth timeout");
                    });
                };
                socket.OnClose = () => {
                    if (AllSockets.TryRemove(socket.ConnectionInfo.Id, out _)) {
                        Log.Info($"Socket {socket.ConnectionInfo.Id} disconnected");
                    }
                };
                socket.OnMessage = message => {
                    if (socket.ConnectionInfo.Headers[authenticated] == "true") {
                        try {
                            MessageHandler.HandleMessage(message, socket, _gameInv);
                        } catch (Exception e) {
                            Log.Error($"Error handling message: {e}");
                        }
                    } else if (message == WsPass) {
                        socket.ConnectionInfo.Headers[authenticated] = "true";
                    }
                };
            });

            Log.Info($"WebSocket server started on {WsUri}");
            _sleepUntilStopped.WaitOne();
        }

        public void Stop() {
            _gameInv.Inventory.ItemsChanged -= SendItems;

            foreach (var socket in AllSockets.Values) {
                socket.Send(EncodeMessage("disconnect", null, "Server closed"));
                socket.Close();
            }

            _server.Dispose();
            _sleepUntilStopped.Set();
            Log.Info("WebSocket server stopped");
        }

        public GameInv GameInv {
            set {
                if (_gameInv == null!) {
                    _gameInv = value;
                } else {
                    throw new InvalidOperationException("GameInv already set");
                }
            }
        }

        private void SendItems(IWebSocketConnection socket) {
            var serializedItems = JsonConvert.SerializeObject(_gameInv.Inventory.ToList());
            var message = EncodeMessage("items", null, serializedItems);
            socket.Send(message);
        }

        private void SendItems() {
            foreach (var socket in AllSockets.Values) {
                SendItems(socket);
            }
        }
    }
}
