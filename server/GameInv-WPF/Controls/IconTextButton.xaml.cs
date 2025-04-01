using System.Windows;
using iNKORE.UI.WPF.Modern.Common.IconKeys;

namespace GameInv_WPF.Controls {
    public partial class IconTextControl {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(FontIconData), typeof(IconTextControl), new(default(FontIconData)));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconTextControl), new(default(string)));

        public IconTextControl() {
            InitializeComponent();
            DataContext = this;
        }

        public FontIconData Icon {
            get => (FontIconData)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
