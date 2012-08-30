using System;
using System.Windows.Data;

namespace wp7_api_demos.ViewModel.Converters
{
    public class EmptyFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String stringValue = value as String;
            if(String.IsNullOrEmpty(stringValue))
                return "*************";
            else 
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
