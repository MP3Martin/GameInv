using System.Collections;
using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public class Inventory : IInventory {
        private readonly List<Item> _items = [];

        public IEnumerator<Item> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
