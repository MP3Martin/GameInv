using GameInv.Inventory;
using NLog;

namespace GameInv {
    public class GameInv(IInventory inventory) {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IInventory _inventory = inventory;

        public void Run() {
            Logger.Info("Started");
        }

        public void Tick() {
            // TODO: break items over time, remove if durability is <= 0
        }
    }
}
