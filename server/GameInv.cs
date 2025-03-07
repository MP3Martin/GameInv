using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    /// <summary>
    ///     The <i>"main"</i> class
    /// </summary>
    public class GameInv {
        private static readonly Logger Log = GetLogger();
        private readonly IConnectionHandler? _connectionHandler;
        public readonly IInventory Inventory;

        /// <summary>
        ///     The <i>"main"</i> class
        /// </summary>
        public GameInv(IInventory inventory, IConnectionHandler? connectionHandler = null) {
            _connectionHandler = connectionHandler;
            Inventory = inventory;

            if (connectionHandler is not null) {
                StartConnection();
            }
        }

        private void StartConnection() {
            Console.CancelKeyPress += (_, ea) => {
                ea.Cancel = true;
                Log.Info("Stopping...");
                _connectionHandler?.Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            if (_connectionHandler is not null) {
                _connectionHandler.GameInv = this;
            }

            Log.Info($"Starting connection handler ({(_connectionHandler?.GetType().Name ?? "").Pastel(Highlight)})");
            _connectionHandler?.Start();
        }
    }
}
