using GameInv.ItemNS;

namespace GameInv.InventoryNS {
    public interface IInventory : IEnumerable<Item> {
        public void AddItem(Item item);

        public bool ModifyItem(Item item);

        public bool RemoveItem(string id);

        public event Action ItemsChanged;
    }
}
