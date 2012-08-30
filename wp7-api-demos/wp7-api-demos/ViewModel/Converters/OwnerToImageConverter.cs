using System;
using System.Windows.Data;

namespace wp7_api_demos.ViewModel.Converters
{
    public class OwnerToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format("/Resources/icons/User{0}Icon.fw.png", value.ToString().ToLower());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
