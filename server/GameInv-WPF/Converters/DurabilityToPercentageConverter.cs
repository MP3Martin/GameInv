using System.Globalization;
using System.Windows.Data;
using GameInv.ItemNS;

namespace GameInv_WPF.Converters {
    public class DurabilityToPercentageConverter : IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is not ItemDurability durability) return null;
            double percentage = durability / (float)ItemDurability.MaxValue * 100;
            return $"{percentage:0.##}%";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
