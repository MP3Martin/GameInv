using Fleck;

namespace GameInv.Ws {
    public class WebSocketConnectionInterfaceWrapper {
        private const string AuthenticatedString = "Authenticated";
        private readonly IWebSocketConnection _socketConnection;

        public WebSocketConnectionInterfaceWrapper(IWebSocketConnection socketConnection) {
            _socketConnection = socketConnection;
            Authenticated = false;
        }
        public bool Authenticated {
            get => _socketConnection.ConnectionInfo.Headers[AuthenticatedString] == "true";
            set => _socketConnection.ConnectionInfo.Headers[AuthenticatedString] = value.ToString().ToLower();
        }

        public bool IsAvailable => _socketConnection.IsAvailable;

        public IWebSocketConnectionInfo ConnectionInfo => _socketConnection.ConnectionInfo;

        public void Send(string message) {
            if (!Authenticated) return;
            _socketConnection.Send(message);
        }

        public void Close() {
            _socketConnection.Close();
        }
    }
}
