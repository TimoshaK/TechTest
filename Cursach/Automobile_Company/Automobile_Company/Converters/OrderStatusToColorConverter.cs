using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Converters
{
    public class OrderStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.New:
                        return new SolidColorBrush(Colors.Blue);
                    case OrderStatus.WaitingPayment:
                        return new SolidColorBrush(Colors.Orange);
                    case OrderStatus.Paid:
                        return new SolidColorBrush(Colors.Green);
                    case OrderStatus.InProgress:
                        return new SolidColorBrush(Colors.Purple);
                    case OrderStatus.Completed:
                        return new SolidColorBrush(Colors.DarkGreen);
                    case OrderStatus.Cancelled:
                        return new SolidColorBrush(Colors.Red);
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }

            if (value is TripStatus tripStatus)
            {
                switch (tripStatus)
                {
                    case TripStatus.Created:
                        return new SolidColorBrush(Colors.Blue);
                    case TripStatus.Loading:
                        return new SolidColorBrush(Colors.Orange);
                    case TripStatus.OnRoute:
                        return new SolidColorBrush(Colors.Purple);
                    case TripStatus.Unloading:
                        return new SolidColorBrush(Colors.DarkOrange);
                    case TripStatus.Completed:
                        return new SolidColorBrush(Colors.DarkGreen);
                    case TripStatus.Cancelled:
                        return new SolidColorBrush(Colors.Red);
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }

            return new SolidColorBrush(Colors.Gray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}