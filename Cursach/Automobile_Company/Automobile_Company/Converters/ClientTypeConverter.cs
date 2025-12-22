using System;
using System.Globalization;
using System.Windows.Data;
using Automobile_Company.Model;

namespace Automobile_Company.Converters
{
    public class ClientTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Client client)
            {
                if (client is IndividualClient)
                    return "Физ. лицо";
                if (client is LegalClient)
                    return "Юр. лицо";
            }

            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}