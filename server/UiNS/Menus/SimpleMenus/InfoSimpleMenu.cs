namespace CarGarage.Ui.Menus.SimpleMenus {
    public class InfoSimpleMenu(string message) : SimpleMenu {
        protected override string Title => "Info";
        protected override void OnShow() {
            Console.WriteLine(message);

            Pause(ConsoleKey.Enter, true);
        }
    }
}
