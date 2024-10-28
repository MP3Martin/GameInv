using System.Collections;

namespace GameInv.Inventory {
    public class Inventory : IInventory {
        private readonly List<Item.Item> _items = [];

        public IEnumerator<Item.Item> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

   
}
