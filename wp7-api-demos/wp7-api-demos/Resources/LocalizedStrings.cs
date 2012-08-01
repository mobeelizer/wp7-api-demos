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

namespace wp7_api_demos.Resources
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static Resources.ResourceDictionary localizedResources = new Resources.ResourceDictionary();

        public Resources.ResourceDictionary LocalizedResources { get { return localizedResources; } }
    }
}
