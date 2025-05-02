using System.Collections.Concurrent;
using Fleck;
using Newtonsoft.Json;

namespace GameInv.Ws {
    /// <inheritdoc />
    public class WsConnectionHandler : IConnectionHandler {
        private static readonly Logger Log = GetLogger();
        private readonly ConcurrentDictionary<Guid, WebSocketConnectionInterfaceWrapper> _allSockets = new();
        private readonly AutoResetEvent _sleepUntilStopped = new(false);

        private GameInv _gameInv = null!;
        private WebSocketServer _server = null!;

        private string SocketInfo => $"({_allSockets.Count} total)";

        /// <remarks>
        ///     Only call this once per instance
        /// </remarks>
        public void Start() {
            if (_gameInv == null!) {
                throw new InvalidOperationException("GameInv not set");
            }

            _gameInv.Inventory.ItemsChanged += SendItems;

            _server = new(WsUri);
            FleckLog.LogAction = (_, _, _) => { }; // Disable FleckLog logging

            // ReSharper disable once InconsistentNaming
            _server.Start(_socket => {
                // Only use _socket in pre-auth
                var socket = new WebSocketConnectionInterfaceWrapper(_socket);
                _socket.OnOpen = () => {
                    if (!_allSockets.TryAdd(socket.ConnectionInfo.Id, socket)) return;
                    Log.Info($"Socket {socket.ConnectionInfo.Id} connected " + SocketInfo);

                    // Set a timeout to disconnect if no auth received
                    Task.Run(async () => {
                        await Task.Delay(5000);
                        if (!socket.IsAvailable || socket.Authenticated) return;
                        FailAuth(_socket);
                    });
                };
                _socket.OnClose = () => {
                    if (_allSockets.TryRemove(socket.ConnectionInfo.Id, out _)) {
                        Log.Info($"Socket {socket.ConnectionInfo.Id} disconnected " + SocketInfo);
                    }
                };
                _socket.OnMessage = message => {
                    if (socket.Authenticated) {
                        try {
                            MessageHandler.HandleMessage(message, socket, _gameInv);
                        } catch (Exception e) {
                            Log.Error($"Error handling message: {e}");
                        }
                    } else if (message == WsPass) {
                        // Authenticated
                        socket.Authenticated = true;

                        SendItems(socket);
                    } else {
                        FailAuth(_socket);
                    }
                };
            });

            Log.Info($"WebSocket server started on {WsUri}");
            _sleepUntilStopped.WaitOne();
        }

        public void Stop() {
            _gameInv.Inventory.ItemsChanged -= SendItems;

            foreach (var socket in _allSockets.Values) {
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
        private static void FailAuth(IWebSocketConnection socket) {
            socket.Send(EncodeMessage("disconnect", null, "Failed auth"));
            socket.Close();
            Log.Info($"Socket {socket.ConnectionInfo.Id} failed auth");
        }

        private void SendItems(WebSocketConnectionInterfaceWrapper socket) {
            var items = _gameInv.Inventory.ToList();
            var itemsData = items.Select(i => (ItemData)i);
            var serializedItems = JsonConvert.SerializeObject(itemsData);
            var message = EncodeMessage("items", null, serializedItems);
            socket.Send(message);
        }

        private void SendItems() {
            foreach (var socket in _allSockets.Values) {
                SendItems(socket);
            }
        }
    }
}
