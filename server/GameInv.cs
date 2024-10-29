using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    public class GameInv(IInventory inventory, IConnectionHandler connectionHandler) {
        private static readonly Logger Log = GetLogger();
        private IInventory _inventory = inventory;

        public void Run() {
            Console.CancelKeyPress += (_, cea) => {
                cea.Cancel = true;
                Log.Info("Stopping...");
                Stop();
                Log.Info("Bye.");
                Environment.Exit(0);
            };

            Log.Info("Instance created");

            Log.Info($"Starting connection handler ({connectionHandler.GetType().Name.Pastel(Highlight)})");
            connectionHandler.Start();
        }

        public void Stop() {
            connectionHandler.Stop();
        }
    }
}
