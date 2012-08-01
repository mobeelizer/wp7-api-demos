using System;
using System.Windows.Data;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel.Converters
{
    public class User2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            User user = (User)value;
            switch (user)
            {
                case User.B:
                    return "User B";
                default:
                case User.A:
                    return "User A";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
