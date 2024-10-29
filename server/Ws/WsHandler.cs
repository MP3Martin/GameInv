namespace GameInv.Ws {
    /// <inheritdoc />
    public class SignalRConnectionHandler : IConnectionHandler {
        private static readonly Logger Log = GetLogger();
        private readonly AutoResetEvent _sleepUntilStopped = new(false);

        private GameInv _gameInv = null!;
        public void Start() {
            if (_gameInv == null!) {
                throw new InvalidOperationException("GameInv not set");
            }
            
            // *start the sever*
            Log.Info("SignalR server started");
            _sleepUntilStopped.WaitOne(); // remove this if the SignalR server is blocking by itself
        }

        public void Stop() {
            // *stop the sever*
            _sleepUntilStopped.Set();
            Log.Info("SignalR server stopped");
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
