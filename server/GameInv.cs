using GameInv.InventoryNS;

namespace GameInv {
    public class GameInv(IInventory inventory) {
        private static readonly Logger Log = GetLogger();
        private IInventory _inventory = inventory;

        public void Run() {
            Log.Info("Instance created");
        }

        public void Tick() {
            // TODO: break items over time, remove if durability is <= 0
        }
    }
}
