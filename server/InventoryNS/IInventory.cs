using GameInv.Db;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public interface IInventory : IEnumerable<Item> {
        IItemDataSource? ItemDataSource { get; }

        public void AddItem(Item item);

        /// <inheritdoc cref="UseItem(string,out bool)" />
        public bool UseItem(Item item, out bool itemBroke);
        /// <returns>Success</returns>
        public bool UseItem(string id, out bool itemBroke);

        public void TickTime(int tickCount);

        /// <returns>True if the item was successfully modified</returns>
        public bool ModifyItem(Item item);

        /// <returns>True if the item was successfully removed</returns>
        public bool RemoveItem(string id);

        /// <inheritdoc cref="RemoveItem(string)" />
        public bool RemoveItem(Item item);

        public int GetItemIndex(string id);

        public event Action ItemsChanged;
    }
}
