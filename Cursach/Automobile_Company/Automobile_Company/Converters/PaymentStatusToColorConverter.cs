using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Converters
{
    public class PaymentStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PaymentStatus status)
            {
                switch (status)
                {
                    case PaymentStatus.Pending:
                        return new SolidColorBrush(Color.FromArgb(255, 255, 193, 7)); // Amber
                    case PaymentStatus.Paid:
                        return new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)); // Green
                    case PaymentStatus.PartiallyPaid:
                        return new SolidColorBrush(Color.FromArgb(255, 255, 152, 0)); // Orange
                    case PaymentStatus.Overdue:
                        return new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)); // Red
                    case PaymentStatus.Cancelled:
                        return new SolidColorBrush(Color.FromArgb(255, 158, 158, 158)); // Gray
                    default:
                        return new SolidColorBrush(Color.FromArgb(255, 96, 125, 139)); // Blue Gray
                }
            }

            return new SolidColorBrush(Color.FromArgb(255, 96, 125, 139)); // Blue Gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}