using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Helpers;
using iNKORE.UI.WPF.Modern.Controls;

namespace GameInv_WPF.WPF {
    public static class Utils {
        /// <param name="element">Must be <see cref="TextBox" /> or <see cref="NumberBox" /></param>
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
