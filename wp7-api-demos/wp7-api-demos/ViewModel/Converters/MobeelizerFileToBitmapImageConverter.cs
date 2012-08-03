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
using System.Windows.Media.Imaging;

namespace wp7_api_demos.ViewModel.Converters
{
    public class MobeelizerFileToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IMobeelizerFile file = value as IMobeelizerFile;
            BitmapImage bitmap = new BitmapImage();
            bitmap.CreateOptions = BitmapCreateOptions.None;
            using (var stream = file.GetStream())
            {
                bitmap.SetSource(stream);
            }

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
