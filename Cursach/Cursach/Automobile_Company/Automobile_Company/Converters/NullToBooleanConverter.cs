using System;
using System.Globalization;
using System.Windows.Data;

namespace Automobile_Company.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = value == null;

            // Если передан параметр "inverse", инвертируем результат
            if (parameter != null && parameter.ToString().ToLower() == "inverse")
            {
                return !isNull;
            }

            return !isNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}