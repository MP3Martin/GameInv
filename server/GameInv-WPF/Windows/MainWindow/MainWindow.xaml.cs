using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GameInv_WPF.Dialogs;
using GameInv.ItemNS;
using MessageBox = System.Windows.MessageBox;
using static GameInv_WPF.Windows.MainWindow.Utils;

namespace GameInv_WPF.Windows.MainWindow {
    public partial class MainWindow {
        private readonly GameInv.GameInv _gameInv = null!;

        public MainWindow() {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            _gameInv = Init.Initialize();
            Items = (ObservableCollection<Item>)_gameInv.Inventory.Items;

            DataContext = this;
        }

        public ObservableCollection<Item> Items { get; } = null!;

        private void ExitButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void AddItemButton_OnClick(object sender, RoutedEventArgs e) {
            HandleAddItem();
        }

        private void RemoveItemButton_OnClick(object sender, RoutedEventArgs e) {
            if (ItemsDataGrid.SelectedItem is not Item item) return;

            if (MessageBox.Show($"Are you sure you want to remove {item.Name}?", "Remove Item", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            if (!_gameInv.Inventory.RemoveItem(item)) {
                ShowErrorMessageBox("Failed to remove item.");
            }
        }

        private void ItemsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            // Prevent double-click on a row when double-clicking a button
            var hit = VisualTreeHelper.HitTest(ItemsDataGrid, e.GetPosition(ItemsDataGrid));
            if (hit is not null) {
                var clickedElement = hit.VisualHit;
                while (clickedElement is not null && clickedElement != ItemsDataGrid) {
                    if (clickedElement is Button) {
                        e.Handled = true;
                        return;
                    }

                    clickedElement = VisualTreeHelper.GetParent(clickedElement);
                }
            }

            if (((FrameworkElement)e.OriginalSource).DataContext is not Item item) return;

            HandleModifyItem(item);
        }

        private void UseItemButton_OnClick(object sender, RoutedEventArgs e) {
            if (e.OriginalSource is not Button button) return;
            if (button.CommandParameter is not Item item) return;

            if (!item.Usable) {
                return;
            }

            if (!_gameInv.Inventory.UseItem(item, out var itemBroke)) {
                ShowErrorMessageBox("Failed to use item.");
                return;
            }

            if (itemBroke) return;

            Dispatcher.InvokeAsync(() => {
                var newButton = GetButtonForItem(item, "UseItemButton", this);
                newButton?.Focus();
            }, DispatcherPriority.Render);

            ItemsDataGrid.SelectedItem = item;
        }

        private void TickTimeButton_OnClick(object sender, RoutedEventArgs e) {
            var selectedItem = ItemsDataGrid.SelectedItem;

            new TickTimeDialog(_gameInv) {
                Owner = this
            }.ShowDialog();

            if (ItemsDataGrid.Items.Contains(selectedItem)) {
                ItemsDataGrid.SelectedItem = selectedItem;
            }
        }

        private void EditItemButton_OnClick(object sender, RoutedEventArgs e) {
            if (e.OriginalSource is not Button button) return;
            if (button.CommandParameter is not Item item) return;

            HandleModifyItem(item);
        }

        private void ItemsDataGrid_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.OriginalSource is not DataGridCell) return;
            if (e.Key is not (Key.Enter or Key.Space)) return;

            if (ItemsDataGrid.SelectedItem is not Item selectedItem) return;
            e.Handled = true;
            HandleModifyItem(selectedItem);
        }

        private void HandleModifyItem(Item item) {
            var selectedIndex = ItemsDataGrid.SelectedIndex;

            var dialog = new ItemEditDialog(item) {
                Owner = this
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != true) return;

            var resultItem = dialog.ResultItem!;
            if (!_gameInv.Inventory.ModifyItem(resultItem)) {
                ShowErrorMessageBox("Failed to save item. Changes not saved.");
                return;
            }

            ItemsDataGrid.SelectedIndex = selectedIndex;
            SelectItemEditButton(resultItem, this);
        }

        private void HandleAddItem() {
            var dialog = new ItemEditDialog {
                Owner = this
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != true) return;

            var resultItem = dialog.ResultItem!;
            if (!_gameInv.Inventory.AddItem(resultItem)) {
                ShowErrorMessageBox("Failed to add item. Changes not saved.");
                return;
            }

            ItemsDataGrid.SelectedItem = resultItem;
            SelectItemEditButton(resultItem, this);
        }
    }
}
