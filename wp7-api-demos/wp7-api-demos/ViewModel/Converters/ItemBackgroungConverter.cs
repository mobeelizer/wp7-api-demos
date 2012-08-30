using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.ViewModel.Converters
{
    public class ItemBackgroungConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MobeelizerWp7Model item = value as MobeelizerWp7Model;
            if (item.Conflicted)
            {
                return "#4BED0000";
            }
            else if (item.Modified)
            {
                return "#4C2D8000";
            }
            else
            {
                return "#4C000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
