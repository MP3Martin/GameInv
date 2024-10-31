using GameInv.ConsoleUiNS.Menus;

namespace GameInv.ConsoleUiNS {
    public class ConsoleUi {
#pragma warning disable CA1822
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Start(GameInv gameInv) {
#pragma warning restore CA1822
            new MainMenu(gameInv).Show();
        }
    }
    
}
