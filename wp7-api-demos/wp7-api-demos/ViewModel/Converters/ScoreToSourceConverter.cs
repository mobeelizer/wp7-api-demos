using System;
using System.Windows.Data;

namespace wp7_api_demos.ViewModel.Converters
{
    public class ScoreToSourceConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format("/Resources/icons/star_{0}.fw.png", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
