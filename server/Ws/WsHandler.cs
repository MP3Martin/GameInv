using System.Collections.Concurrent;
using Fleck;
using LogLevel = Fleck.LogLevel;
using static GameInv.Ws.MessageDataTools;

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

            const string uri = "ws://0.0.0.0:9081";
            _server = new(uri);
            FleckLog.Level = LogLevel.Error;
            _server.Start(socket => {
                socket.OnOpen = () => {
                    if (AllSockets.TryAdd(socket.ConnectionInfo.Id, socket)) {
                        Log.Info($"Socket {socket.ConnectionInfo.Id} connected");
                    }
                };
                socket.OnClose = () => {
                    if (AllSockets.TryRemove(socket.ConnectionInfo.Id, out _)) {
                        Log.Info($"Socket {socket.ConnectionInfo.Id} disconnected");
                    }
                };
                socket.OnMessage = message => {
                    MessageHandler.HandleMessage(message, socket, _gameInv);
                };
            });

            Log.Info($"WebSocket server started on {uri}");
            _sleepUntilStopped.WaitOne();
        }

        public void Stop() {
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
    }
}
