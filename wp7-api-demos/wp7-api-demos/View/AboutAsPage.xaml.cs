using System;
using Microsoft.Phone.Controls;

namespace wp7_api_demos.View
{
    public partial class AboutAsPage : PhoneApplicationPage
    {
        private string sessionCode;

        public AboutAsPage()
        {
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
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(String.Format("/View/SimpleSyncPage.xaml?SessionCode={0}", sessionCode), UriKind.Relative));
        }
    }
}