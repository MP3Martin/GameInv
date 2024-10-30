using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    public class GameInv(IInventory inventory, IConnectionHandler connectionHandler) {
        private static readonly Logger Log = GetLogger();
        public readonly IInventory Inventory = inventory;

        public void Start() {
            Console.CancelKeyPress += (_, cea) => {
                cea.Cancel = true;
                Log.Info("Stopping...");
                Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            connectionHandler.GameInv = this;
            Log.Info($"Starting connection handler ({connectionHandler.GetType().Name.Pastel(Highlight)})");
            connectionHandler.Start();
        }

        private void Stop() {
            connectionHandler.Stop();
        }
    }
}
