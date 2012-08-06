using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Coding4Fun.Phone.Controls;
using wp7_api_demos.ViewModel;
using wp7_api_demos.View.Controls.info;

namespace wp7_api_demos.View
{
    public partial class RelationConflictsPage : PhoneApplicationPage, INavigationService
    {
        private RelationConflictsPageViewModel viewModel;

        public RelationConflictsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PageLoaded);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!PageSettings.PageOpened(ExamplePage.RelationConflicts))
            {
                OnInfoClicked(this, null);
                PageSettings.SetPageOpened(ExamplePage.RelationConflicts);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                String sessioCode = this.NavigationContext.QueryString["SessionCode"];
                this.viewModel = new RelationConflictsPageViewModel(this, Int32.Parse(sessioCode));
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

        private void OnAddClicked(object sender, EventArgs e)
        {
            if (this.viewModel.AddCommand != null)
            {
                this.viewModel.AddCommand.Execute(null);
            }
        }

        private void OnSyncClicked(object sender, EventArgs e)
        {
            if (this.viewModel.SyncCommand != null)
            {
                this.viewModel.SyncCommand.Execute(null);
            }
        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            MessagePrompt messagePrompt = new MessagePrompt();
            messagePrompt.Title = wp7_api_demos.Resources.ResourceDictionary.dialogRelationConflictsSyncTitle;
            messagePrompt.Body = new RelationConflictsInfoMessage();
            messagePrompt.Show();
        }

        private void OnLogout(object sender, EventArgs e)
        {
            this.viewModel.LogoutCommand.Execute(null);
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(String.Format("/View/PushNotificationPage.xaml?SessionCode={0}", viewModel.SessionCode), UriKind.Relative));
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