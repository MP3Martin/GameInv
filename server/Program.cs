global using Sherlog;
global using static System.Drawing.Color;
global using static GameInv.Utils;
global using static GameInv.Consts;
using System.Drawing;
using GameInv.InventoryNS;
using Pastel;
using static GameInv.Consts.Colors;

namespace GameInv {
    public static class Program {
        private static readonly Logger Log = GetLogger();
        public static void Main(string[] args) {
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
                    DateTime.UtcNow.ToString("yyyy-MM-dd").Pastel(LessImportantText) +
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
            new GameInv(
                new Inventory()
            ).Run();

            Thread.Sleep(-1);
        }
    }
}
