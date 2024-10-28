
using NLog;

namespace GameInv {
    public static class Program {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args) {
            Logger.Info("Creating a ");
            new GameInv(
                new Inventory.Inventory()
            ).Run();
            Logger.Info("sus");
            
            Thread.Sleep(-1);
        }
    }
}
