using System;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;
using System.Windows;

namespace wp7_api_demos.View
{
    public partial class SelectScorePage : PhoneApplicationPage, INavigationService
    {
        private SelectScorePageViewModel viewModel;

        public SelectScorePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("guid"))
            {
                String guid = this.NavigationContext.QueryString["guid"];
                this.viewModel = new SelectScorePageViewModel(this, guid);
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
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }));
        }


        public void GoBackToRoot()
        {
            throw new NotImplementedException();
        }
    }
}