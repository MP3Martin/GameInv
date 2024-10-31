using GameInv.ItemNS;

namespace GameInv.UiNS.Menus.SimpleMenus {
    public class AddItemSimpleMenu(GameInv gameInv) : SimpleMenu {
        protected override string Title => "Add an item";
        protected override void OnShow() {
            var item = new Item(
                Prompt("Enter name: "),
                PromptParse<ushort>("Enter damage per tick (empty to skip)", emptyReturnsNull: true),
                PromptParse<ushort>("Enter damage per use (empty to skip)", emptyReturnsNull: true)
            );
            gameInv.Inventory.AddItem(item);
        }
    }
}
