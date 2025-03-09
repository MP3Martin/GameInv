using GameInv.Db;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public interface IInventory : IEnumerable<Item> {
        IItemDataSource? ItemDataSource { get; }

        public void AddItem(Item item);

        /// <inheritdoc cref="UseItem(string,out bool)" />
        public bool UseItem(Item item, out bool itemBroke);
        /// <returns>True if the item was successfully used</returns>
        public bool UseItem(string id, out bool itemBroke);

        /// <returns>True if the time got ticked successfully</returns>
        public bool TickTime(int tickCount);

        /// <returns>True if the item was successfully modified</returns>
        public bool ModifyItem(Item item);

        /// <returns>True if the item was successfully removed</returns>
        public bool RemoveItem(Item item, bool noLog = false);
        /// <inheritdoc cref="RemoveItem(Item,bool)" />
        public bool RemoveItem(string id, bool noLog = false);

        public int GetItemIndex(string id);

        public event Action ItemsChanged;
    }
}
