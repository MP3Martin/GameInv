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

        public bool UseItem(Item item, out bool itemBroke) {
            return UseItem(item.Id, out itemBroke);
        }

        public bool UseItem(string id, out bool itemBroke) {
            itemBroke = false;

            var index = GetItemIndex(id);
            if (index == -1) return false;
            var item = _items[index];

            if (!item.Usable) return false;

            var useResult = item._Use();

            if (useResult) /* The item broke */ {
                _items.Remove(item);
                itemBroke = true;
            } else {
                itemBroke = false;
            }

            Log.Info($"Item \"{item.Name}\" used");
            if (itemBroke) {
                Log.Info($"Item \"{item.Name}\" broke");
            }
            ItemsChanged?.Invoke();

            return true;
        }

        public void TickTime(int tickCount) {
            var items = _items.ToArray();
            foreach (var item in items) {
                if (!item.Decays) continue;
                if (item._TickDurability(tickCount)) /* The item broke */ {
                    _items.Remove(item);
                }
            }

            Log.Info("Time ticked");
            ItemsChanged?.Invoke();
        }

        public bool RemoveItem(Item item) {
            return RemoveItem(item.Id);
        }

        public bool RemoveItem(string id) {
            var index = GetItemIndex(id);
            if (index == -1) return false;

            var name = _items[index].Name;
            _items.RemoveAt(index);

            Log.Info($"Item \"{name}\" removed");
            ItemsChanged?.Invoke();

            return true;
        }

        public bool ModifyItem(Item item) {
            var index = GetItemIndex(item.Id);
            if (index == -1) return false;

            _items.RemoveAt(index);
            _items.Insert(index, item);

            Log.Info($"Item \"{item.Name}\" modified");
            ItemsChanged?.Invoke();

            return true;
        }

        public int GetItemIndex(string id) {
            return _items.FindIndex(i => i.Id == id);
        }
    }
}
