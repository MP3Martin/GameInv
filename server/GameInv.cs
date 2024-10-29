using GameInv.InventoryNS;
using GameInv.Ws;
using WebSocketSharp.Server;

namespace GameInv {
    public class GameInv(IInventory inventory, IConnectionHandler connectionHandler) {
        private static readonly Logger Log = GetLogger();
        private IInventory _inventory = inventory;

        public void Run() {
            Log.Info("Instance created");
            
            connectionHandler.Start();
        }

        public void Stop() {
            connectionHandler.Stop();
        }
    }
}
