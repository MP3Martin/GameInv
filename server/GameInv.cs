using NLog;

namespace GameInv {
    public class GameInv(IInventory inventory) {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IInventory _inventory = inventory;

        public void Run() {
            _logger.Info("Started");
        }

        public void Tick() {
            // TODO: break items over time, remove if durability is <= 0
        }
    }
}
