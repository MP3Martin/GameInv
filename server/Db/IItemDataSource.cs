using GameInv.ItemNS;

namespace GameInv.Db {
    public interface IItemDataSource {
        string ConnectionString { get; init; }

        public IEnumerable<Item> GetItems();

        public bool UpdateItem(Item item);

        public bool RemoveItem();
    }
}
