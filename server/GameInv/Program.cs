global using Sherlog;
global using static System.Drawing.Color;
global using static GameInv.UtilsNS.Consts;
global using static GameInv.UtilsNS.Consts.Colors;
global using static GameInv.UtilsNS.Utils;
global using static GameInv.Ws.MessageDataTools;
using GameInv.ConsoleUiNS;
using GameInv.Db;
using GameInv.UtilsNS.ErrorPresenterNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    public static class Program {
        private static readonly Logger Log = GetLogger();
        private static readonly ConsoleErrorPresenter ErrorPresenter = new();

        public static void Main(string[] args) {
            MyEnv.LoadEnv();

            var envUseWsServer = MyEnv.GetBool("USE_WS_SERVER");
            var useWsServer = envUseWsServer ??
                YesNoInput(
                    "Use WebSocket server",
                    "Use console UI",
                    true
                );

            if (envUseWsServer is null) Console.WriteLine();
            var useDb = MyEnv.GetBool("USE_DB") ??
                YesNoInput(
                    "Use MySQL DB",
                    "Don't use DB (will lose state on exit)",
                    true
                );
            ClearAll();

            var dbConnectionString = MyEnv.GetString("DB_CONNECTION_STRING");
            if (useDb && dbConnectionString is null) {
                ErrorPresenter.Present(string.Format(Errors.NoDbConnectionString, EnvPrefix), pause: true);
                Environment.Exit(1);
            }

            IItemDataSource? itemDataSource = useDb
                ? new MySqlItemDataSource {
                    ConnectionString = dbConnectionString!
                }
                : null;

            if (useWsServer) {
                InitLogger();

                Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

                try {
                    _ = new GameInv(ErrorPresenter, new WsConnectionHandler(),
                        itemDataSource
                    );
                } catch (Exception e) {
                    Log.Error(e.ToString());
                    Environment.Exit(1);
                }
            } else /* Console UI */ {
                Log.LogLevel = LogLevel.Fatal; // Disable logging

                var gameInv = new GameInv(ErrorPresenter, itemDataSource: itemDataSource);

                new ConsoleUi().Start(gameInv);
                Environment.Exit(0);
            }
        }
    }
}
