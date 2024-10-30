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

            _gameInv.Inventory.ItemsChanged += SendItems;


            _server = new(WsUri);
            FleckLog.Level = LogLevel.Debug;
            _server.Start(socket => {
                socket.OnOpen = () => {
                    if (!AllSockets.TryAdd(socket.ConnectionInfo.Id, socket)) return;
                    Log.Info($"Socket {socket.ConnectionInfo.Id} connected");

                    SendItems(socket);
                };
                socket.OnClose = () => {
                    if (AllSockets.TryRemove(socket.ConnectionInfo.Id, out _)) {
                        Log.Info($"Socket {socket.ConnectionInfo.Id} disconnected");
                    }
                };
                socket.OnMessage = message => {
                    try {
                        MessageHandler.HandleMessage(message, socket, _gameInv);
                    } catch (Exception e) {
                        Log.Error($"Error handling message: {e}");
                    }
                };
            });

            Log.Info($"WebSocket server started on {WsUri}");
            _sleepUntilStopped.WaitOne();
        }

        public void Stop() {
            _gameInv.Inventory.ItemsChanged -= SendItems;

            foreach (var socket in AllSockets.Values) {
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
            var message = EncodeMessage("items", Guid.NewGuid().ToString(), serializedItems);
            socket.Send(message);
        }

        private void SendItems() {
            foreach (var socket in AllSockets.Values) {
                SendItems(socket);
            }
        }
    }
}
