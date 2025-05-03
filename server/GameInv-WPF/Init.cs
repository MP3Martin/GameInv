global using static GameInv_WPF.UtilsNS.Utils;
global using static GameInv.UtilsNS.Consts.Colors;
global using static GameInv.UtilsNS.Utils;
using System.Runtime.InteropServices;
using GameInv_WPF.UtilsNS;
using Pastel;
using Sherlog;

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

            CheckDbConnectionString(ErrorPresenter);

            Log.Info($"Creating a new instance of {nameof(GameInv).Pastel(Highlight)}...");

            try {
                return new(ErrorPresenter);
            } catch (Exception e) {
                ErrorPresenter.Present(e.ToString());
                Environment.Exit(1);
            }

            return null!;
        }
    }
}
