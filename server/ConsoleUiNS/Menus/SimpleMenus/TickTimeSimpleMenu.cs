namespace GameInv.ConsoleUiNS.Menus.SimpleMenus {
    public class TickTimeSimpleMenu(GameInv gameInv) : SimpleMenu {
        protected override string Title => "Simulate the passage of time";
        protected override void OnShow() {
            var tickCount = PromptParse<int>("How many game ticks do you want to simulate? ");
            gameInv.Inventory.TickTime(tickCount);
        }
    }
}
