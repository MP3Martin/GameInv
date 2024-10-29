using WebSocketSharp.Server;

namespace GameInv.Ws {
    public class WsHandler : IConnectionHandler {
        private static readonly Logger Log = GetLogger();
        private readonly AutoResetEvent _sleepUntilStopped = new(false);
        private WebSocketServer _wssv = null!;
        public void Start() {
            _wssv = new(9081);
            _wssv.Start();
            Log.Info("Websocket server started");
            _sleepUntilStopped.WaitOne();
        }

        public void Stop() {
            _wssv.Stop();
            _sleepUntilStopped.Set();
            Log.Info("Websocket server stopped");
        }
    }
}
