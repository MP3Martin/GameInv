using GameInv.InventoryNS;
using WebSocketSharp.Server;

namespace GameInv {
    public class GameInv(IInventory inventory) {
        private static readonly Logger Log = GetLogger();
        private IInventory _inventory = inventory;
        private WsConnection _wsConnection = new();

        public void Run() {
            Log.Info("Instance created");
            
            _wsConnection.Start();
        }

        public void Stop() {
            _wsConnection.Stop();
        }
    }
}
