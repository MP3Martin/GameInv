using GameInv.ConsoleUiNS.Menus.SimpleMenus;

namespace GameInv.ConsoleUiNS.Menus {
    public class MainMenu : MenuPage {
        public MainMenu(GameInv gameInv) {
            Options = [
                ("Add an item", () => new AddItemSimpleMenu(gameInv).Show()),
                ("Remove an item", () => new ChooseAnItemToRemoveMenu(gameInv).Show()),
                ("Show all items", () => new ListItemsSimpleMenu(gameInv).Show()),
                (ExitMenuString, ExitMenu)
            ];
        }

        protected override string Title => "Main menu";
    }
}
