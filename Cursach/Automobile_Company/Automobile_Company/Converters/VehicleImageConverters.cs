using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Automobile_Company.Converters
{
    public class VehicleImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = value as string;

            // Если есть путь к изображению
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                    // Если не удалось загрузить - возвращаем дефолтное
                    return GetDefaultImage();
                }
            }

            // Если нет изображения - возвращаем дефолтное
            return GetDefaultImage();
        }

        private BitmapImage GetDefaultImage()
        {
            try
            {
                // Путь к дефолтному изображению
                return new BitmapImage(new Uri("/Resources/default.jpg", UriKind.RelativeOrAbsolute));
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}