using System.Collections;
using GameInv.Db;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public class Inventory(IItemDataSource? itemDataSource = null) : IInventory {
        private static readonly Logger Log = GetLogger();
        private readonly List<Item> _items = [];
        public IItemDataSource? ItemDataSource { get; } = itemDataSource;

        public event Action? ItemsChanged;

        public IEnumerator<Item> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void AddItem(Item item) {
            if (ItemDataSource is not null) {
                if (!ItemDataSource.UpdateItem(item)) return;
            }

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

            Item? oldItem = null;
            if (ItemDataSource is not null) {
                oldItem = (Item)item.Clone();
            }

            var useResult = item._Use();
            itemBroke = useResult;
            if (itemBroke) {
                if (RemoveItem(item, true)) return false;
            }

            if (ItemDataSource is not null) {
                if (!ItemDataSource.UpdateItem(item)) {
                    Log.Error("Failed to save used item to DB, undoing.");
                    _items[index] = oldItem!;
                    return false;
                }
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

                if (ItemDataSource is not null) {
                    _ = ItemDataSource.UpdateItem(item);
                } // TODO
            }

            Log.Info("Time ticked");
            ItemsChanged?.Invoke();
        }

        public bool RemoveItem(Item item, bool noLog = false) {
            var index = _items.IndexOf(item);
            if (index == -1) return false;

            if (ItemDataSource is not null) {
                Log.Error("Failed to remove item from DB, undoing.");
                var result = ItemDataSource.RemoveItem(_items[index]);
                if (!result) return false;
            }

            var name = _items[index].Name;
            _items.RemoveAt(index);

            if (!noLog) Log.Info($"Item \"{name}\" removed");
            ItemsChanged?.Invoke();

            return true;
        }

        public bool RemoveItem(string id, bool noLog = false) {
            return RemoveItem(_items[GetItemIndex(id)], noLog);
        }

        public bool ModifyItem(Item item) {
            var index = GetItemIndex(item.Id);
            if (index == -1) return false;


            if (ItemDataSource is not null) {
                var oldItem = (Item)item.Clone();

                if (!ItemDataSource.UpdateItem(_items[index])) {
                    _items[index] = oldItem;
                    return false;
                }
            }

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
