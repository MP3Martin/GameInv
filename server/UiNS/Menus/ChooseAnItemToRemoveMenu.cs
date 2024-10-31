using GameInv.UiNS.Menus.SimpleMenus;

namespace GameInv.UiNS.Menus {
    public class ChooseAnItemToRemoveMenu : MenuPage {
        public ChooseAnItemToRemoveMenu(GameInv gameInv) {
            Options = ItemsAsMenuOptions(gameInv.Inventory.ToArray(), item => {
                var menu = new RemoveAnItemSimpleMenu(gameInv, item);
                menu.Show();
                if (!menu.Cancelled) {
                    ExitMenu();
                }
            });
        }

        protected override string Title => "Choose an item to remove";
    }
}
