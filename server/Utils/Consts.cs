using System.Drawing;

namespace GameInv.Utils {
    public static class Consts {
        public const int TicksPerSecond = 20;
        public const string LogTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static class Colors {
            public static readonly Color Highlight = DeepSkyBlue;
            public static readonly Color MiscChar = Gray;
            public static readonly Color LessImportantText = LightGray;
            public static readonly Color ImportantText = White;
        }
    }
}
