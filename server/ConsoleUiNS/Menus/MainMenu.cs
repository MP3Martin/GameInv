using GameInv.ConsoleUiNS.Menus.SimpleMenus;
using static GameInv.ConsoleUiNS.Menus.ChooseAnItemMenu.ActionName;

namespace GameInv.ConsoleUiNS.Menus {
    public class MainMenu : MenuPage {
        private readonly GameInv _gameInv;
        public MainMenu(GameInv gameInv) {
            _gameInv = gameInv;

            Options = [
                ("Add an item", () => new AddItemSimpleMenu(gameInv).Show()),
                ("Show all items", () => new ListItemsSimpleMenu(gameInv).Show()),
                ("Use an item", () => new ChooseAnItemMenu(gameInv, Use, gameInv.Inventory.Where(i => i.Usable)).Show()),
                ("Simulate game time", () => new TickTimeSimpleMenu(gameInv).Show()),
                ("Remove an item", () => new ChooseAnItemMenu(gameInv, Remove).Show()),
                (ExitMenuString, ExitMenu)
            ];
        }

        protected override string Title => "GameInv main menu";
    }
}
