using GameInv.ItemNS;

namespace GameInv.UiNS.Menus.SimpleMenus {
    public class RemoveAnItemSimpleMenu(GameInv gameInv, Item item) : SimpleMenu {
        public bool Cancelled;
        protected override string Title => "Remove an item";

        protected override void OnShow() {
            Console.WriteLine(item + "\n");
            if (YesNoInput("Are you sure you want to remove this item?", false)) {
                gameInv.Inventory.RemoveItem(item);
                ShowInfo($"Item {item.Name} removed.");
            } else {
                Cancelled = true;
            }
        }
    }
}
