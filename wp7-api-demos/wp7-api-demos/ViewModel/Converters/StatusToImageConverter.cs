﻿using System;
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

namespace wp7_api_demos.ViewModel.Converters
{
    public class StatusToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int statusCode = (int)value;
            switch (statusCode)
            {
                case 1:
                    return "/Resources/icons/ic_status_new.png";
                case 2:
                    return "/Resources/icons/ic_status_pending.png";
                case 3:
                    return "/Resources/icons/ic_status_readytoship.png";
                case 4:
                    return "/Resources/icons/ic_status_shipped.png";
                default:
                case 5:
                    return "/Resources/icons/ic_status_received.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
