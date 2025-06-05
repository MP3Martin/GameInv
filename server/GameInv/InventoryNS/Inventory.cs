using System.Collections;
using GameInv.DataSource;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public class Inventory(IItemDataSource? itemDataSource = null) : IInventory {
        private static readonly Logger Log = GetLogger();
        private readonly RefreshableObservableCollection<Item> _items = [];

        private IItemDataSource? ItemDataSource { get; } = itemDataSource;

        public IReadOnlyList<Item> Items => _items;

        public event Action? ItemsChanged;

        public IEnumerator<Item> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool AddItem(Item item, bool noLog = false) {
            if (ItemDataSource is not null) {
                if (!ItemDataSource.UpdateItem(item)) {
                    Log.Error($"Failed to save new item to {ItemDataSource.SourceName}, undoing.");
                    return false;
                }
            }

            _items.Add(item);

            if (!noLog) Log.Info($"Item \"{item.Name}\" added");
            ItemsChanged?.Invoke();

            return true;
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
            _items.RefreshItem(item);
            itemBroke = useResult;
            if (itemBroke) {
                if (!RemoveItem(item, true)) return false;
            }

            if (ItemDataSource is not null && !itemBroke) {
                if (!ItemDataSource.UpdateItem(item)) {
                    Log.Error($"Failed to save used item to {ItemDataSource.SourceName}, undoing.");
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

        public bool TickTime(int tickCount) {
            var items = _items.ToArray();

            Dictionary<Item, ItemDurability>? modifiedItemsPreviousDurabilities = null;
            Dictionary<Item, int>? removedItemsIndexes = null;

            if (ItemDataSource is not null) {
                modifiedItemsPreviousDurabilities = new();
                removedItemsIndexes = new();
            }

            foreach (var item in items) {
                if (!item.Decays) continue;
                var itemDurability = item.Durability;
                if (item._TickDurability(tickCount)) /* The item broke */ {
                    if (ItemDataSource is not null) removedItemsIndexes?.Add(item, _items.IndexOf(item));
                    _items.Remove(item);
                } else /* The item didn't break */ {
                    if (ItemDataSource is not null) modifiedItemsPreviousDurabilities?.Add(item, (ItemDurability)itemDurability!);
                    _items.RefreshItem(item);
                }
            }

            if (ItemDataSource is not null) {
                if (!ItemDataSource.RemoveItems(removedItemsIndexes!.Keys) || !ItemDataSource.UpdateItems(modifiedItemsPreviousDurabilities!.Keys)) {
                    Log.Error("Failed to tick time, undoing");

                    foreach (var (item, index) in removedItemsIndexes) {
                        _items.Insert(index, item);
                    }

                    foreach (var (item, durability) in modifiedItemsPreviousDurabilities!) {
                        _items[_items.IndexOf(item)]._SetDurability(durability);
                        _items.RefreshItem(item);
                    }

                    return false;
                }
            }

            Log.Info($"Time ticked for {tickCount} ticks");
            ItemsChanged?.Invoke();

            return true;
        }

        public bool RemoveItem(Item item, bool noLog = false) {
            var index = _items.IndexOf(item);
            if (index == -1) return false;

            if (ItemDataSource is not null) {
                if (!ItemDataSource.RemoveItem(_items[index])) {
                    Log.Error($"Failed to remove item from {ItemDataSource.SourceName}, undoing.");
                    return false;
                }
            }

            var name = _items[index].Name;
            _items.RemoveAt(index);

            if (!noLog) Log.Info($"Item \"{name}\" removed");
            ItemsChanged?.Invoke();

            return true;
        }

        public bool RemoveItem(string id, bool noLog = false) {
            var index = GetItemIndex(id);
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (index == -1) return false;

            return RemoveItem(_items[index], noLog);
        }

        public bool ModifyItem(Item item) {
            var index = GetItemIndex(item.Id);
            if (index == -1) return false;

            if (ItemDataSource is not null) {
                if (!ItemDataSource.UpdateItem(item)) {
                    Log.Error($"Failed to save modified item to {ItemDataSource.SourceName}, undoing.");
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
            for (var i = 0; i < _items.Count; i++) {
                if (_items[i].Id == id) {
                    return i;
                }
            }

            return -1;
        }
    }
}
