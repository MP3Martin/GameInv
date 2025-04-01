using GameInv.ItemNS;

namespace GameInv.ConsoleUiNS.Menus.SimpleMenus {
    public class AddItemSimpleMenu(GameInv gameInv) : SimpleMenu {
        protected override string Title => "Add an item";
        protected override void OnShow() {
            var item = new Item(
                Prompt("Enter name: "),
                PromptParse<ushort>("Enter damage per tick (empty to skip): ", true),
                PromptParse<ushort>("Enter damage per use (empty to skip): ", true)
            );
            gameInv.Inventory.AddItem(item);
        }
    }
}
