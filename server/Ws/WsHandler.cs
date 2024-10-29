using System.Collections.Concurrent;
using System.Data;
using Fleck;
using LogLevel = Fleck.LogLevel;

namespace GameInv.Ws {
    /// <inheritdoc />
    public class WsHandler : IConnectionHandler {
        private static readonly Logger Log = GetLogger();
        private readonly AutoResetEvent _sleepUntilStopped = new(false);

        private GameInv _gameInv = null!;
        private WebSocketServer _server = null!;
        private static readonly ConcurrentDictionary<Guid, IWebSocketConnection> AllSockets = new ConcurrentDictionary<Guid, IWebSocketConnection>();
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
                    HandleMessage(message, socket);
                };
            });

            Log.Info($"WebSocket server started on {uri}");
            _sleepUntilStopped.WaitOne();
        }

        private static void HandleMessage(string message, IWebSocketConnection socket) {
            var messageParts = message.Split('|');
            string commandType;
            string commandUuid;
            string commandData;
            try {
                commandType = messageParts[0];
                commandUuid = messageParts[1];
                commandData = messageParts[2];
            } catch (IndexOutOfRangeException) { return; }


            switch (commandType) {
                case "RemoveItem":
                {
                    socket.Send("pong");
                    break;
                }
            }
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
