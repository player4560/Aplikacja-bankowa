using System;
using System.Globalization;
using System.Windows.Data;

namespace KoKaBank
{
    public class AmountToColorConverter : IValueConverter
    {
        public static AmountToColorConverter Instance { get; } = new AmountToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                return amount < 0; // True dla ujemnych (czerwony), False dla dodatnich (zielony)
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}