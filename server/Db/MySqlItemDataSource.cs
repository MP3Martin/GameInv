using GameInv.ItemNS;

namespace GameInv.Db {
    public class MySqlItemDataSource : IItemDataSource {
        public required string ConnectionString { get; init; }

        public IEnumerable<Item>? GetItems() {
            throw new NotImplementedException();
        }

        public IEnumerable<Item>? GetItem() {
            throw new NotImplementedException();
        }

        public bool UpdateItem() {
            throw new NotImplementedException();
        }

        public bool RemoveItem() {
            throw new NotImplementedException();
        }
    }
}
