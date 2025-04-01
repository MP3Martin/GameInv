using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using iNKORE.UI.WPF.Helpers;
using iNKORE.UI.WPF.Modern.Controls;
using MessageBox = System.Windows.MessageBox;

namespace GameInv_WPF.UtilsNS {
    public static class Utils {
        public static DependencyObject? FindChildByName(DependencyObject parent, string name) {
            // Iterate through all children of the parent
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is the correct type and name, return it
                if (child is FrameworkElement fe && fe.Name == name) {
                    return child;
                }

                // Recurse into the child if it's a container
                var result = FindChildByName(child, name);
                if (result is not null) {
                    return result;
                }
            }

            return null;
        }

        public static MessageBoxResult ShowErrorMessageBox(string message) {
            return MessageBox.Show(message, "Error");
        }

        /// <param name="element">Must be <see cref="TextBox" /> or <see cref="iNKORE.UI.WPF.Modern.Controls.NumberBox" /></param>
        /// <param name="window"></param>
        /// <param name="setIsInteracted"></param>
        public static void AutoSelectTextBoxOnRender(FrameworkElement element, Window window, Action<bool>? setIsInteracted = null) {
            if (element is not (TextBox or NumberBox)) {
                throw new ArgumentException("Element must be a TextBox or NumberBox.");
            }

            window.ContentRendered += (_, _) => {
                var textBox = element switch {
                    TextBox textBoxElement => textBoxElement,
                    NumberBox numberBox => numberBox.FindVisualChild<TextBox>(),
                    _ => throw new ArgumentException("Element must be a TextBox or NumberBox.")
                };

                textBox.Focus();
                textBox.CaretIndex = textBox.Text.Length;
                textBox.SelectAll();

                setIsInteracted?.Invoke(false);
            };
        }
    }
}
