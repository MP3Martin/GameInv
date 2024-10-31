using GameInv.UiNS.Menus.SimpleMenus;

namespace GameInv.UiNS.Menus {
    public class MainMenu : MenuPage {
        public MainMenu(GameInv gameInv) {
            Options = [
                ("Add an item", new AddItemSimpleMenu(gameInv).Show),
                ("Remove an item", new ChooseAnItemToRemoveMenu(gameInv).Show),
                // ("Show all cars", ShowMenu<CarListSimpleMenu>),
                // ("Use a car", ShowMenu<ChooseACarToGoOnATripMenu>),
                // ("Refuel a car", ShowMenu<RefuelACarMenu>),
                // ("Edit a car", ShowMenu<ChooseACarToEditMenu>),
                ("Remove a car", () => { }),
                (ExitMenuString, ExitMenu)
            ];
        }

        protected override string Title => "Main menu";
    }
}
