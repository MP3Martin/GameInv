using GameInv.ItemNS;

namespace GameInv.DataSource {
    public interface IItemDataSource {
        public string SourceName { get; }

        /// <returns>Items if successful, else null</returns>
        public IEnumerable<Item>? GetItems(out string? errorMessage);

        /// <returns>Success</returns>
        public bool UpdateItem(Item item);

        /// <inheritdoc cref="UpdateItem" />
        public bool UpdateItems(IEnumerable<Item> items);

        /// <inheritdoc cref="UpdateItem" />
        public bool RemoveItem(Item item);

        /// <inheritdoc cref="UpdateItem" />
        public bool RemoveItems(IEnumerable<Item> items);
    }
}
