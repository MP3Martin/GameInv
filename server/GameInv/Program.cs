global using Sherlog;
global using static System.Drawing.Color;
global using static GameInv.UtilsNS.Consts;
global using static GameInv.UtilsNS.Consts.Colors;
global using static GameInv.UtilsNS.Utils;
global using static GameInv.Ws.MessageDataTools;
using GameInv.ConsoleUiNS;
using GameInv.UtilsNS.ErrorPresenter;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    public static class Program {
        private static readonly Logger Log = GetLogger();
        private static readonly ConsoleErrorPresenter ErrorPresenter = new();

        public static void Main(string[] args) {
            MyEnv.LoadEnv();

            var useWsServer = MyEnv.GetBool("USE_WS_SERVER") ??
                YesNoInput(
                    "Use WebSocket server",
                    "Use console UI",
                    true
                );
            ClearAll();

            CheckDbConnectionString(ErrorPresenter);

            if (useWsServer) {
                InitLogger();

                Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

                try {
                    _ = new GameInv(ErrorPresenter, new WsConnectionHandler());
                } catch (Exception e) {
                    ErrorPresenter.Present(e.ToString());
                    Environment.Exit(1);
                }
            } else /* Console UI */ {
                Log.LogLevel = LogLevel.Fatal; // Disable logging

                var gameInv = new GameInv(ErrorPresenter);

                new ConsoleUi().Start(gameInv);

                gameInv.OnClosing();
                Environment.Exit(0);
            }
        }
    }
}
