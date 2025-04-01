using System.ComponentModel;
using System.Windows;

namespace GameInv_WPF.WPF.Dialogs {
    public partial class TickTimeDialog {
        private readonly GameInv _gameInv;
        private int _tickCount;

        public TickTimeDialog(GameInv gameInv) {
            InitializeComponent();

            _gameInv = gameInv;

            Utils.AutoSelectTextBoxOnRender(TickAmountNumberBox, this);
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            if (DialogResult == true) {
                _gameInv.Inventory.TickTime(_tickCount);
            }
        }

        private bool CheckData() {
            var ok = int.TryParse(TickAmountNumberBox.Text, out _tickCount) && _tickCount >= 0;

            if (ok) return true;

            MessageBox.Show("Invalid input. Only whole numbers allowed.");
            return false;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            if (CheckData()) DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
