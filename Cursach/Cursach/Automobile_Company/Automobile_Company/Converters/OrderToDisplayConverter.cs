using System;
using System.Globalization;
using System.Windows.Data;
using Automobile_Company.Model;

namespace Automobile_Company.Converters
{
    public class OrderToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Order order)
            {
                return $"Заказ #{order.Id} - {order.Sender.GetClientInfo()} → {order.Receiver.GetClientInfo()}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}