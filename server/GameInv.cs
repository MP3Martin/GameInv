using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    /// <summary>
    ///     The <i>"main"</i> class
    /// </summary>
    public class GameInv(IInventory inventory, IConnectionHandler? connectionHandler = null) {
        private static readonly Logger Log = GetLogger();
        public readonly IInventory Inventory = inventory;

        public void Start() {
            Console.CancelKeyPress += (_, ea) => {
                ea.Cancel = true;
                Log.Info("Stopping...");
                Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            if (connectionHandler is not null) {
                connectionHandler.GameInv = this;
            }

            Log.Info($"Starting connection handler ({(connectionHandler?.GetType().Name ?? "").Pastel(Highlight)})");
            connectionHandler?.Start();
        }

        private void Stop() {
            connectionHandler?.Stop();
        }
    }
}
