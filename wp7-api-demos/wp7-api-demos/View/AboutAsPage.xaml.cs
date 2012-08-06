using System;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class AboutAsPage : PhoneApplicationPage, INavigationService
    {
        private AboutPageViewModel viewModel;

        private string sessionCode;

        public AboutAsPage()
        {
            this.viewModel = new AboutPageViewModel(this);
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                this.sessionCode = this.NavigationContext.QueryString["SessionCode"];
            }

            base.OnNavigatedTo(e);
        }

        private void OnLogout(object sender, EventArgs e)
        {
            this.viewModel.LogoutCommand.Execute(null);
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(String.Format("/View/SimpleSyncPage.xaml?SessionCode={0}", sessionCode), UriKind.Relative));
        }

        public void Navigate(Uri path)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string title, string message)
        {
            throw new NotImplementedException();
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
    }
}