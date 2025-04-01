using System.Drawing;

namespace GameInv_WPF.UtilsNS {
    public static class Consts {
        public const string ExitMenuString = "Exit";
        public const string GoBackMenuString = "Go back";

        public const string LogTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public const string EnvPrefix = "GAMEINV_";
        public static readonly string WsUri = "ws://0.0.0.0:9081";
        public static readonly string WsPass = "changeme";
        public static readonly Color ExitMenuColor = Goldenrod;

        static Consts() {
            WsUri = MyEnv.GetString("WS_URI") ?? WsUri;
            WsPass = MyEnv.GetString("WS_PASS") ?? WsPass;
        }

        public static class Colors {
            public static readonly Color Highlight = DeepSkyBlue;
            public static readonly Color MiscChar = Gray;
            public static readonly Color LessImportantText = LightGray;
            public static readonly Color ImportantText = White;
        }
    }
}
