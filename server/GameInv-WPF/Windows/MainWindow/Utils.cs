using System.Windows.Controls;
using System.Windows.Threading;
using GameInv.ItemNS;

namespace GameInv_WPF.Windows.MainWindow {
    public static class Utils {
        public static void SelectItemEditButton(Item item, MainWindow mainWindow) {
            mainWindow.Dispatcher.InvokeAsync(() => {
                var newButton = GetButtonForItem(item, "EditItemButton", mainWindow);
                newButton?.Focus();
            }, DispatcherPriority.Render);
        }

        /// Find the use button corresponding to the item
        /// Yes, this is ugly, but I didn't find a better solution (skill issue)
        public static Button? GetButtonForItem(Item item, string name, MainWindow mainWindow) {
            if (mainWindow.ItemsDataGrid.ItemContainerGenerator.ContainerFromItem(item) is not DataGridRow row) return null;

            var column = (DataGridTemplateColumn)mainWindow.ItemsDataGrid.Columns[4];
            var cell = column.GetCellContent(row);
            if (cell is null) return null;
            var button = FindChildByName(cell, name);
            return button as Button;
        }
    }
}
