using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GameInv_WPF.ItemNS;
using iNKORE.UI.WPF.Modern.Controls.Primitives;
using MessageBox = System.Windows.MessageBox;

namespace GameInv_WPF.WPF.Dialogs {
    public partial class ItemEditDialog {
        private bool _isInteracted;
        public ItemEditDialog(Item? item = null) {
            InitializeComponent();

            var isEditing = item is not null;
            Title = isEditing ? "Edit item" : "Add item";
            PreviewMouseUp += OnUserInteraction;
            PreviewKeyUp += OnUserInteraction;
            if (isEditing) DurabilityNumberBox.IsEnabled = false;

            Utils.AutoSelectTextBoxOnRender(NameTextBox, this, SetIsInteracted);

            if (item is null) return;
            ResultItem = item;

            // Load values from item
            NameTextBox.Text = item.Name;
            if (item.DamagePerTick is not null) DamagePerTickNumberBox.Value = (double)item.DamagePerTick;
            if (item.DamagePerUse is not null) DamagePerUseNumberBox.Value = (double)item.DamagePerUse;
            if (item.Durability is not null) DurabilityNumberBox.Value = (double)item.Durability;
        }
        public Item? ResultItem { get; private set; }
        private void SetIsInteracted(bool value) {
            _isInteracted = value;
        }

        private void OnUserInteraction(object sender, InputEventArgs e) {
            // Allow clicking cancel/close buttons without setting _isInteracted if not interacted with anything else yet
            if (!_isInteracted) {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (e.OriginalSource is Button button && button == CancelButton) return;
                if (e.OriginalSource is TitleBarButton { Name: "CloseButton" }) return;
            }

            _isInteracted = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            if (DialogResult == true || !_isInteracted) return; // Don't show confirmation if save pressed, or if the user has not changed anything
            var confirmationResult = MessageBox.Show("Do you really want to cancel?\n\nData won't be saved.",
                "Confirmation", MessageBoxButton.OKCancel);
            if (confirmationResult == MessageBoxResult.Cancel) {
                e.Cancel = true;
            }
        }

        private bool CheckData() {
            // Validate NameTextBox
            if (string.IsNullOrEmpty(NameTextBox.Text)) {
                ShowErrorMessageBox("Invalid name format");
                return false;
            }

            // Validate other fields
            Console.WriteLine(DamagePerTickNumberBox.Value);
            if (!TryParseItemDurability(DamagePerTickNumberBox.Value, "Invalid damage per tick format", out var damagePerTick) ||
                !TryParseItemDurability(DamagePerUseNumberBox.Value, "Invalid damage per use format", out var damagePerUse) ||
                !TryParseItemDurability(DurabilityNumberBox.Value, "Invalid durability format", out var durability)) {
                return false;
            }

            ResultItem = new(
                NameTextBox.Text,
                damagePerTick,
                damagePerUse,
                durability,
                ResultItem?.Id);

            return true;

            // Validate and show error messages for each field
            bool TryParseItemDurability(double value, string errorMessage, out ushort? result) {
                result = null;
                if (double.IsNaN(value)) {
                    return true;
                }

                if (ushort.TryParse(value.ToString(CultureInfo.InvariantCulture), out var ushortResult)) {
                    result = ushortResult;
                    return true;
                }

                ShowErrorMessageBox(errorMessage);
                return false;
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            if (CheckData()) DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
