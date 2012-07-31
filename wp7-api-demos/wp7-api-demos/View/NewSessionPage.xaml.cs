using System;
using System.Windows;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class NewSessionPage : PhoneApplicationPage, INavigationService
    {
        private NewSessionPageViewModel viewModel;

        public NewSessionPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                String sessioCode = this.NavigationContext.QueryString["SessionCode"];
                this.viewModel = new NewSessionPageViewModel(this, Int32.Parse(sessioCode));
                this.LayoutRoot.DataContext = this.viewModel;
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
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }));
        }
    }
}