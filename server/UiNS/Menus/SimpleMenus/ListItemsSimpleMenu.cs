namespace GameInv.UiNS.Menus.SimpleMenus {
    public class ListItemsSimpleMenu(GameInv gameInv) : SimpleMenu {
        protected override string Title => "List of items";
        protected override void OnShow() { 
            var items = gameInv.Inventory.Select(i => i.ToString()).ToArray();
            Console.WriteLine(string.Join("--------------\n", items));
            
            Pause();
        }
    }
}
