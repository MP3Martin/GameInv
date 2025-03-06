global using Sherlog;
global using static System.Drawing.Color;
global using static GameInv.UtilsNS.Consts;
global using static GameInv.UtilsNS.Consts.Colors;
global using static GameInv.UtilsNS.Utils;
global using static GameInv.Ws.MessageDataTools;
using System.Drawing;
using GameInv.ConsoleUiNS;
using GameInv.InventoryNS;
using GameInv.Ws;
using Pastel;

namespace GameInv {
    public static class Program {
        private static readonly Logger Log = GetLogger();
        public static void Main(string[] args) {
            LoadEnv();

            var useWsServer = YesNoInput(
                """
                Y - Use WebSocket server
                N - Use console UI


                """, true);
            ClearAll();

            if (useWsServer) {
                var logLevelColorMap = new Dictionary<LogLevel, Color> {
                    { LogLevel.Trace, Cyan },
                    { LogLevel.Debug, Blue },
                    { LogLevel.Info, White },
                    { LogLevel.Warn, Orange },
                    { LogLevel.Error, Red },
                    { LogLevel.Fatal, Magenta }
                };

                Logger.AddAppender((logger, level, message) => {
                    message = "[".Pastel(MiscChar) +
                        DateTime.Now.ToString(LogTimeFormat).Pastel(LessImportantText) +
                        " " +
                        level.ToString().Pastel(logLevelColorMap[level]) +
                        " (".Pastel(MiscChar) +
                        string.Join('.', logger.Name.Split('.')[1..]).Pastel(LessImportantText) +
                        ")".Pastel(MiscChar) +
                        "]".Pastel(MiscChar) +
                        " " +
                        message.Pastel(logLevelColorMap[level]);
                    Console.WriteLine(message);
                });

                Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

                var gameInv = new GameInv(
                    new Inventory(),
                    new WsHandler()
                );
                try {
                    gameInv.Start();
                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            } else /* Console ui */ {
                Log.LogLevel = LogLevel.Fatal; // Disable logging

                var gameInv = new GameInv(new Inventory());

                new ConsoleUi().Start(gameInv);
                Environment.Exit(0);
            }
        }
    }
}
