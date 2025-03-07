﻿global using Sherlog;
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

                try {
                    _ = new GameInv(
                        new Inventory(),
                        new WsConnectionHandler()
                    );
                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            } else /* Console UI */ {
                Log.LogLevel = LogLevel.Fatal; // Disable logging

                var gameInv = new GameInv(new Inventory());

                new ConsoleUi().Start(gameInv);
                Environment.Exit(0);
            }
        }
    }
}
