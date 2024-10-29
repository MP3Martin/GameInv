using GameInv.InventoryNS;
using WebSocketSharp.Server;

namespace GameInv {
    public class GameInv(IInventory inventory) {
        private static readonly Logger Log = GetLogger();
        private IInventory _inventory = inventory;

        public void Run() {
            Log.Info("Instance created");
            
            var wssv = new WebSocketServer (4649);
        }

        public void Tick() {
            // TODO: break items over time, remove if durability is <= 0
        }
    }
}
