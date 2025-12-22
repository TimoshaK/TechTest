using Automobile_Company.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Automobile_Company.Converters
{
    public class CanEditDriverAssignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return false;

            var vehicle = values[0] as Vehicle;
            var driver = values[1] as Driver;

            // Проверяем, можно ли редактировать
            if (!vehicle.IsAvailable)
                return false;

            if (driver != null && !driver.IsAvailable)
                return false;

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}