global using static System.Drawing.Color;
global using static GameInv_WPF.UtilsNS.Consts;
global using static GameInv_WPF.UtilsNS.Consts.Colors;
global using static GameInv_WPF.UtilsNS.Utils;
global using static GameInv_WPF.Ws.MessageDataTools;
using System.Runtime.InteropServices;
using System.Windows;
using GameInv_WPF.Db;
using Pastel;
using Sherlog;

namespace GameInv_WPF {
    public static class Init {
        private static readonly Logger Log = GetLogger();

        [DllImport("kernel32.dll")]
#pragma warning disable SYSLIB1054
        private static extern bool AttachConsole(int dwProcessId);
#pragma warning restore SYSLIB1054
        public static GameInv Initialize() {
            MyEnv.LoadEnv();
            AttachConsole(-1);
            InitLogger();
            Log.Info("Starting...");

            var useDb = MyEnv.GetBool("USE_DB");
            if (useDb is null) {
                switch (MessageBox.Show("Use database?",
                            "Question", MessageBoxButton.YesNoCancel)) {
                    case MessageBoxResult.Cancel or MessageBoxResult.None:
                    {
                        Environment.Exit(0);
                        break;
                    }
                    case MessageBoxResult.Yes or MessageBoxResult.OK:
                    {
                        useDb = true;
                        break;
                    }
                    case MessageBoxResult.No:
                    {
                        useDb = false;
                        break;
                    }
                }

                useDb ??= null!;
            }

            var dbConnectionString = MyEnv.GetString("DB_CONNECTION_STRING");
            if ((bool)useDb && dbConnectionString is null) {
                const string error = $"No DB connection string set.\n" +
                    $"Set it using DB_CONNECTION_STRING in .env file or using the {EnvPrefix}DB_CONNECTION_STRING environment variable.";
                Log.Error(error);
                ShowErrorMessageBox(error);
                Environment.Exit(1);
            }

            IItemDataSource? itemDataSource = (bool)useDb
                ? new MySqlItemDataSource {
                    ConnectionString = dbConnectionString!
                }
                : null;

            Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

            try {
                return new(itemDataSource);
            } catch (Exception e) {
                var msg = e.ToString();
                Log.Error(msg);
                ShowErrorMessageBox(msg);
                Environment.Exit(1);
            }

            return null!;
        }
    }
}
