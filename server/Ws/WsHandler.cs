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

        private void HandleMessage(string message, IWebSocketConnection socket) {
            if (!MessageDataTools.Decode(message, out var commandType, out var messageUuid, out var commandData)) return;

            void Ok(bool success, string? infoMessage = null) {
                socket.Send(new MessageDataTools.SuccessData
                        { Success = success, Message = infoMessage }.Serialize()
                );
            }

            switch (commandType) {
                case "add_item":
                {
                    if (MessageDataTools.ModifiedItem.Deserialize(commandData, out var itemId, out var itemAmount)) { }

                    break;
                }
                case "remove_item":
                {
                    // ReSharper disable once InlineTemporaryVariable
                    var itemId = commandData;
                    _gameInv.Inventory.RemoveItem(itemId);
                    socket.Send(MessageDataTools.Encode("Confirm", messageUuid, "ok"));
                    break;
                }
            }
        }
    }
}
