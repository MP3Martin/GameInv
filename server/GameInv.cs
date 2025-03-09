using GameInv.Db;
using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    /// <summary>
    ///     The <i>"main"</i> class
    /// </summary>
    public class GameInv {
        private static readonly Logger Log = GetLogger();
        private readonly IConnectionHandler? _clientConnectionHandler;
        private readonly IItemDataSource? _itemDataSource;
        public readonly IInventory Inventory;

        /// <summary>
        ///     The <i>"main"</i> class
        /// </summary>
        public GameInv(IConnectionHandler? clientConnectionHandler = null, IItemDataSource? itemDataSource = null) {
            _clientConnectionHandler = clientConnectionHandler;
            _itemDataSource = itemDataSource;

            Inventory = new Inventory(itemDataSource);

            if (_clientConnectionHandler is not null) {
                StartClientConnection();
            }

            if (_itemDataSource is not null) {
                StartItemDataSource();
            }
        }

        private void StartItemDataSource() {
            if (_itemDataSource is null) return;

            var items = _itemDataSource.GetItems();
            if (items is null) {
                Console.Clear();
                Console.WriteLine($"Couldn't get items from {_itemDataSource.SourceName}. " +
                    $"Make sure everything is running and correctly set up.");
                Pause(newLine: true);
                Environment.Exit(0);
            }

            foreach (var item in items) {
                Inventory.AddItem(item);
            }
        }

        private void StartClientConnection() {
            if (_clientConnectionHandler is null) return;

            Console.CancelKeyPress += (_, ea) => {
                ea.Cancel = true;
                Log.Info("Stopping...");
                _clientConnectionHandler.Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            _clientConnectionHandler.GameInv = this;

            Log.Info($"Starting connection handler ({_clientConnectionHandler.GetType().Name.Pastel(Highlight)})");
            _clientConnectionHandler.Start();
        }
    }
}
