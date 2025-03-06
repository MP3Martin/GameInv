using GameInv.ItemNS;

namespace GameInv.Db {
    public interface IItemDataSource {
        string ConnectionString { get; init; }

        public IEnumerable<Item>? GetItems();
        public IEnumerable<Item>? GetItem();
        public bool UpdateItem();
        public bool RemoveItem();
    }
}
