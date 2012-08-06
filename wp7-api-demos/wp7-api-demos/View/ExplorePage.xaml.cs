using System;
using System.Windows;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class ExplorePage : PhoneApplicationPage, INavigationService
    {
        private ExplorePageViewModel viewModel;

        public ExplorePage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (this.NavigationService.BackStack.GetEnumerator().MoveNext())
            {
                this.NavigationService.RemoveBackEntry();
            }

            base.OnBackKeyPress(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                String sessioCode = this.NavigationContext.QueryString["SessionCode"];
                this.viewModel = new ExplorePageViewModel(this, Int32.Parse(sessioCode));
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
            int howMany = 0;
            foreach (var item in this.NavigationService.BackStack)
            {
                ++howMany;
            }

            for (int i = 0; i < howMany - 1; ++i)
            {
                this.NavigationService.RemoveBackEntry();
            }

            this.NavigationService.GoBack();
        }


        private void OnLogout(object sender, EventArgs e)
        {
            this.viewModel.LogoutCommand.Execute(null);
        }
    }
}