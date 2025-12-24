using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Converters
{
    public class OrderStatusToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.New:
                        return new SolidColorBrush(Color.FromArgb(20, 33, 150, 243)); // Light Blue
                    case OrderStatus.WaitingPayment:
                        return new SolidColorBrush(Color.FromArgb(20, 255, 152, 0)); // Light Orange
                    case OrderStatus.Paid:
                        return new SolidColorBrush(Color.FromArgb(20, 76, 175, 80)); // Light Green
                    case OrderStatus.InProgress:
                        return new SolidColorBrush(Color.FromArgb(20, 156, 39, 176)); // Light Purple
                    case OrderStatus.Completed:
                        return new SolidColorBrush(Color.FromArgb(20, 56, 142, 60)); // Light Dark Green
                    case OrderStatus.Cancelled:
                        return new SolidColorBrush(Color.FromArgb(20, 244, 67, 54)); // Light Red
                    default:
                        return new SolidColorBrush(Colors.White);
                }
            }

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}