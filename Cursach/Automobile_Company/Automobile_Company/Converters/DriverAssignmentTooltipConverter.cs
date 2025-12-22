using Automobile_Company.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Automobile_Company.Converters
{
    public class DriverAssignmentTooltipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return string.Empty;

            var vehicle = values[0] as Vehicle;
            var driver = values[1] as Driver;

            if (!vehicle.IsAvailable)
                return "Невозможно изменить водителя: автомобиль находится в рейсе!";

            if (driver != null && !driver.IsAvailable)
                return "Невозможно изменить водителя: текущий водитель находится в рейсе!";

            return "Нажмите для изменения водителя";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}