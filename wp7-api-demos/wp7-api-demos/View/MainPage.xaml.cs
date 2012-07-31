using Microsoft.Phone.Controls;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.ViewModel;
using System.Windows;

namespace wp7_api_demos
{
    public partial class MainPage : PhoneApplicationPage, INavigationService
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            viewModel = new MainPageViewModel(this);
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public void Navigate(System.Uri path)
        {
            this.NavigationService.Navigate(path);
        }

        public void GoBack()
        {
            this.NavigationService.GoBack();
        }


        public void ShowMessage(string title, string message)
        {
            this.Dispatcher.BeginInvoke(new System.Action(()=>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }));
        }
    }
}