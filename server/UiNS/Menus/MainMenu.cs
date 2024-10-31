using GameInv.UiNS.Menus.SimpleMenus;

namespace GameInv.UiNS.Menus {
    public class MainMenu : MenuPage {
        public MainMenu(GameInv gameInv) {
            Options = [
                ("Add an item", () => new AddItemSimpleMenu(gameInv).Show()),
                ("Remove an item", () => new ChooseAnItemToRemoveMenu(gameInv).Show()),
                (ExitMenuString, ExitMenu)
            ];
        }

        protected override string Title => "Main menu";
    }
}
