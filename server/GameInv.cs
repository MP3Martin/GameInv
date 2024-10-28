namespace GameInv {
    public class GameInv(IInventory inventory) {
        private IInventory _inventory = inventory;

        public void Run() {
            
        }

        public void Tick() {
            // TODO: break items over time, remove if durability is <= 0
        }
    }
}
