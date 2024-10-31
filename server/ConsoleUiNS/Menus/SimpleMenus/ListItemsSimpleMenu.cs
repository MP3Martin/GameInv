using Pastel;

namespace GameInv.ConsoleUiNS.Menus.SimpleMenus {
    public class ListItemsSimpleMenu(GameInv gameInv) : SimpleMenu {
        protected override string Title => "List of items";
        protected override void OnShow() {
            var items = gameInv.Inventory.Select(i => i.ToString()).ToArray();
            Console.WriteLine(string.Join(new string('-', 30).Pastel(Gray) + "\n", items));

            Pause();
        }
    }
}
