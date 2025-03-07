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
        public readonly IInventory Inventory;

        /// <summary>
        ///     The <i>"main"</i> class
        /// </summary>
        public GameInv(IInventory inventory, IConnectionHandler? clientConnectionHandler = null, IItemDataSource? itemDataSource = null) {
            _clientConnectionHandler = clientConnectionHandler;
            Inventory = inventory;

            if (clientConnectionHandler is not null) {
                StartClientConnection();
            }

            if (itemDataSource is not null) {
                StartItemDataSource();
            }
        }

        private void StartItemDataSource() {
// TODO
        }

        private void StartClientConnection() {
            Console.CancelKeyPress += (_, ea) => {
                ea.Cancel = true;
                Log.Info("Stopping...");
                _clientConnectionHandler?.Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            if (_clientConnectionHandler is not null) {
                _clientConnectionHandler.GameInv = this;
            }

            Log.Info($"Starting connection handler ({(_clientConnectionHandler?.GetType().Name ?? "").Pastel(Highlight)})");
            _clientConnectionHandler?.Start();
        }
    }
}
