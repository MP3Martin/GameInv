using System.Collections;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public class Inventory : IInventory {
        private static readonly Logger Log = GetLogger();
        private readonly List<Item> _items = [];

        public event Action? ItemsChanged;

        public IEnumerator<Item> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void AddItem(Item item) {
            _items.Add(item);

            Log.Info($"Item \"{item.Name}\" added");
            ItemsChanged?.Invoke();
        }

        /// <returns>True if the item was successfully removed</returns>
        public bool RemoveItem(string id) {
            var index = GetItemIndex(id);
            if (index == -1) return false;

            var name = _items[index].Name;
            _items.RemoveAt(index);

            Log.Info($"Item \"{name}\" removed");
            ItemsChanged?.Invoke();

            return true;
        }

        /// <returns>True if the item was successfully modified</returns>
        public bool ModifyItem(Item item) {
            var index = GetItemIndex(item.Id);
            if (index == -1) return false;

            _items.RemoveAt(index);
            _items.Insert(index, item);

            Log.Info($"Item \"{item.Name}\" modified");
            ItemsChanged?.Invoke();

            return true;
        }

        private int GetItemIndex(string id) {
            return _items.FindIndex(i => i.Id == id);
        }
    }
}
