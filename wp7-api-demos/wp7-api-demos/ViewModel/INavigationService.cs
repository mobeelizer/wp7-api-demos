using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wp7_api_demos.ViewModel
{
    interface INavigationService
    {
        void Navigate(Uri path);

        void GoBack();

        void ShowMessage(String title, String message);
    }
}
