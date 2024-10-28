using System.Collections;

namespace GameInv {
    public class Inventory : IInventory {
        private readonly List<Item> _items = [];

        public IEnumerator<Item> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

   
}
