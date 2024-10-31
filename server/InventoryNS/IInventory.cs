using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public interface IInventory : IEnumerable<Item> {
        public void AddItem(Item item);

        /// <returns>Success</returns>
        public bool UseItem(Item item, out bool itemBroke);

        public void TickTime(int tickCount);

        /// <returns>True if the item was successfully modified</returns>
        public bool ModifyItem(Item item);

        /// <returns>True if the item was successfully removed</returns>
        public bool RemoveItem(string id);

        /// <inheritdoc cref="RemoveItem(string)" />
        public bool RemoveItem(Item item);

        public event Action ItemsChanged;
    }
}
