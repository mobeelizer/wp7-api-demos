using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected INavigationService navigationService;

        protected ICommand logoutCommand;

        private bool isBusy;

        private String busyMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public ICommand LogoutCommand
        {
            get
            {
                if (this.logoutCommand == null)
                {
                    this.logoutCommand = new DelegateCommand(Logout);
                }

                return this.logoutCommand;
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public String BusyMessage
        {
            get
            {
                return this.busyMessage;
            }

            set
            {
                this.busyMessage = value;
                RaisePropertyChanged("BusyMessage");
            }
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void Logout(object arg)
        {
            if (Mobeelizer.CheckSyncStatus().IsRunning())
            {
                this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotLogout);
            }
            else
            {
                Mobeelizer.Logout();
                SessionSettings.RemoveSessionCode();
                this.navigationService.GoBackToRoot();
            }
        }
    }
}
