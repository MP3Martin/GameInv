using WebSocketSharp.Server;

namespace GameInv.Ws {
    public class WsHandler: IConnectionHandler {
        private WebSocketServer _wssv = null!;
        public void Start() {
            _wssv = new (9081);

            _wssv.Start();
            Thread.Sleep(-1);
        }

        public void Stop() {
            _wssv.Stop();
        }
        
    }
}
