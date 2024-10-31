using CarGarage.Ui.Menus.SimpleMenus;

namespace CarGarage.Ui.Menus {
    public class MainMenu : MenuPage {
        public MainMenu() {
            Options = [
                ("Add a car", ShowMenu<AddCarSimpleMenu>),
                ("Show all cars", ShowMenu<CarListSimpleMenu>),
                ("Use a car", ShowMenu<ChooseACarToGoOnATripMenu>),
                ("Refuel a car", ShowMenu<RefuelACarMenu>),
                ("Edit a car", ShowMenu<ChooseACarToEditMenu>),
                ("Remove a car", () => { }),
                (ExitMenuString, ExitMenu)
            ];
        }

        protected override string Title => "Main menu";
    }
}
