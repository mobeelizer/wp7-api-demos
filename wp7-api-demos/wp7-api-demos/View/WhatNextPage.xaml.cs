using System;
using Microsoft.Phone.Controls;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class WhatNextPage : PhoneApplicationPage, INavigationService
    {
        private WhatNextPageViewModel viewModel;

        public WhatNextPage()
        {
            this.viewModel = new WhatNextPageViewModel(this);
            InitializeComponent();
        }

        private void OnLogout(object sender, EventArgs e)
        {
            this.viewModel.LogoutCommand.Execute(null);
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