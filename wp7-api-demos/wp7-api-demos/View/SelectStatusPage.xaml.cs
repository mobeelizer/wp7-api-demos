using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class SelectStatusPage : PhoneApplicationPage, INavigationService
    {
        private SelectStatusViewModel viewModel;

        public SelectStatusPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("guid"))
            {
                String guid = this.NavigationContext.QueryString["guid"];
                this.viewModel = new SelectStatusViewModel(this, guid);
                this.DataContext = this.viewModel;
            }

            base.OnNavigatedTo(e);
        }

        public void Navigate(System.Uri path)
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                this.NavigationService.Navigate(path);
            }));
        }

        public void GoBack()
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                this.NavigationService.GoBack();
            }));
        }


        public void ShowMessage(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}