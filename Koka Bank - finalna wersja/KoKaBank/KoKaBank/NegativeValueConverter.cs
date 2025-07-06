using System;
using System.Globalization;
using System.Windows.Data;

namespace KoKaBank
{
    public class NegativeValueConverter : IValueConverter
    {
        public static NegativeValueConverter Instance { get; } = new NegativeValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // Sprawdź czy tekst zaczyna się od minusa
                return text.StartsWith("-");
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}