using GameInv.DataSource;
using GameInv.InventoryNS;
using GameInv.UtilsNS.ErrorPresenter;
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
        public readonly IErrorPresenter ErrorPresenter;
        public readonly IInventory Inventory;

        public GameInv(IErrorPresenter errorPresenter, IConnectionHandler? clientConnectionHandler = null) {
            _clientConnectionHandler = clientConnectionHandler;
            ErrorPresenter = errorPresenter;

            _itemDataSource = CreateItemDataSource(this);
            Inventory = new Inventory(_itemDataSource);

            if (_itemDataSource is not null) {
                InitializeItemDataSource();
            }

            if (_clientConnectionHandler is not null) {
                StartClientConnectionHandler();
            }
        }
        public event Action? Closing;
        public void OnClosing() {
            Closing?.Invoke();
        }

        private void InitializeItemDataSource() {
            if (_itemDataSource is null) return;

            var items = _itemDataSource.GetItems(out var errorMessage);
            if (items is null) {
                ErrorPresenter.Present($"Couldn't get items from {_itemDataSource.SourceName}. " +
                    $"Make sure everything is running and correctly set up.\n\n{errorMessage}", true);
                Environment.Exit(1);
            }

            items = items.ToArray();

            foreach (var item in items) {
                Inventory.AddItem(item, true);
            }

            Log.Info($"Loaded {items.Count()} items from {_itemDataSource.SourceName}");
        }

        private void StartClientConnectionHandler() {
            if (_clientConnectionHandler is null) return;

            Console.CancelKeyPress += (_, ea) => {
                ea.Cancel = true;
                Log.Info("Stopping...");
                OnClosing();
                _clientConnectionHandler.Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            _clientConnectionHandler.GameInv = this;

            Log.Info($"Starting connection handler ({_clientConnectionHandler.GetType().Name.Pastel(Highlight)})");
            _clientConnectionHandler.Start();
        }
    }
}
