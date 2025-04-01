global using static GameInv_WPF.UtilsNS.Utils;
global using static GameInv.UtilsNS.Consts;
global using static GameInv.UtilsNS.Consts.Colors;
global using static GameInv.UtilsNS.Utils;
using System.Runtime.InteropServices;
using System.Windows;
using GameInv_WPF.UtilsNS;
using GameInv.Db;
using Pastel;
using Sherlog;
using IItemDataSource = GameInv.Db.IItemDataSource;

namespace GameInv_WPF {
    public static class Init {
        private static readonly Logger Log = GetLogger();
        private static readonly MessageBoxErrorPresenter ErrorPresenter = new();

        [DllImport("kernel32.dll")]
#pragma warning disable SYSLIB1054
        private static extern bool AttachConsole(int dwProcessId);
#pragma warning restore SYSLIB1054
        public static GameInv.GameInv Initialize() {
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
                ErrorPresenter.Present(string.Format(Errors.NoDbConnectionString, EnvPrefix)); // TODO: try setting typeof clasnane
                Environment.Exit(1);
            }

            IItemDataSource? itemDataSource = (bool)useDb
                ? new MySqlItemDataSource {
                    ConnectionString = dbConnectionString!
                }
                : null;

            Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

            try {
                return new(ErrorPresenter, itemDataSource: itemDataSource);
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
