using GameInv_WPF.Db;
using GameInv_WPF.InventoryNS;
using Sherlog;

namespace GameInv_WPF {
    /// <summary>
    ///     The <i>"main"</i> class
    /// </summary>
    public class GameInv {
        private static readonly Logger Log = GetLogger();
        private readonly IItemDataSource? _itemDataSource;
        public readonly IInventory Inventory;

        public GameInv(IItemDataSource? itemDataSource = null) {
            _itemDataSource = itemDataSource;

            Inventory = new Inventory(itemDataSource);

            if (_itemDataSource is not null) {
                InitializeItemDataSource();
            }
        }

        private void InitializeItemDataSource() {
            if (_itemDataSource is null) return;

            var items = _itemDataSource.GetItems(out var errorMessage);
            if (items is null) {
                var error = $"Couldn't get items from {_itemDataSource.SourceName}. " +
                    $"Make sure everything is running and correctly set up.\n\n" +
                    $"Error: {errorMessage}";
                Log.Error(error);
                Log.LogLevel = LogLevel.Fatal;
                ShowErrorMessageBox(error);
                Environment.Exit(1);
            }

            items = items.ToArray();

            foreach (var item in items) {
                Inventory.AddItem(item, true);
            }

            Log.Info($"Loaded {items.Count()} items from {_itemDataSource.SourceName}");
        }
    }
}
