using GameInv.ItemNS;

namespace GameInv.Db {
    public interface IItemDataSource {
        string ConnectionString { get; init; }

        public IEnumerable<Item> GetItems();

        /// <returns>Success</returns>
        public bool UpdateItem(Item item);

        /// <returns>Success</returns>
        public bool RemoveItem(Item item);
    }
}
