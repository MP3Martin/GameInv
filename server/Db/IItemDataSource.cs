using GameInv.ItemNS;

namespace GameInv.Db {
    public interface IItemDataSource {
        string ConnectionString { get; init; }

        /// <returns>Items if successful, else null</returns>
        public IEnumerable<Item>? GetItems();

        /// <returns>Success</returns>
        public bool UpdateItem(Item item);

        /// <inheritdoc cref="UpdateItem" />
        public bool UpdateItems(IEnumerable<Item> items);

        /// <inheritdoc cref="UpdateItem" />
        public bool RemoveItem(Item item);
    }
}
