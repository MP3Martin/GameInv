using Fleck;

namespace GameInv.Ws {
    public class MyWebSocketConnection(
        ISocket socket,
        Action<IWebSocketConnection> initialize,
        Func<byte[], WebSocketHttpRequest> parseRequest,
        Func<WebSocketHttpRequest, IHandler> handlerFactory,
        Func<IEnumerable<string>, string> negotiateSubProtocol)
        : WebSocketConnection(socket, initialize, parseRequest, handlerFactory, negotiateSubProtocol) {
        public string Sus = "a";
    }
    
 
}
