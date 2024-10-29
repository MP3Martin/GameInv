using WebSocketSharp.Server;

namespace GameInv {
    public class WsConnection {
        private WebSocketServer _wssv = null!;
        public void Start() {
            _wssv = new (9081);

            _wssv.Start();
        }

        public void Stop() {
            _wssv.Stop();
        }
        
    }
}
