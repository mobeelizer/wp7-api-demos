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
using Coding4Fun.Phone.Controls;
using wp7_api_demos.View.Controls.info;

namespace wp7_api_demos.View
{
    public partial class PermisionsPage : PhoneApplicationPage, INavigationService
    {
        private PermisionsPageViewModel viewModel;

        public PermisionsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                String sessioCode = this.NavigationContext.QueryString["SessionCode"];
                this.viewModel = new PermisionsPageViewModel(this, Int32.Parse(sessioCode));
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
            messagePrompt.Title = wp7_api_demos.Resources.ResourceDictionary.dialogPermissionsSyncTitle;
            messagePrompt.Body = new PermissionsInfoMessage();
            messagePrompt.Show();
        }
    }
}