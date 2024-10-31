using GameInv.ConsoleUiNS.Menus.SimpleMenus;
using GameInv.ItemNS;
using static GameInv.ConsoleUiNS.Menus.ChooseAnItemMenu.ActionName;

namespace GameInv.ConsoleUiNS.Menus {
    public class ChooseAnItemMenu : MenuPage {
        public enum ActionName {
            Use,
            Remove
        }
        private readonly ActionName _actionName;

        public ChooseAnItemMenu(GameInv gameInv, ActionName actionName, IEnumerable<Item>? items = null) {
            _actionName = actionName;

            Dictionary<ActionName, Action<Item>> actionHandlers = new() {
                {
                    Use, item => {
                        gameInv.Inventory.UseItem(item, out var itemBroke);
                        // ReSharper disable once InvertIf
                        if (itemBroke) {
                            ShowInfo($"Item \"{item.Name}\" broke");
                            ExitMenu();
                        }
                    }
                }, {
                    Remove, item => {
                        var menu = new RemoveItemSimpleMenu(gameInv, item);
                        menu.Show();
                        if (!menu.Cancelled) {
                            ExitMenu();
                        }
                    }
                }
            };
            Options = ItemsAsMenuOptions(items ?? gameInv.Inventory.ToArray(), item => actionHandlers[actionName](item));
        }
        protected override string Title => "Choose an item to " + _actionName.ToString().ToLower();
    }
}
