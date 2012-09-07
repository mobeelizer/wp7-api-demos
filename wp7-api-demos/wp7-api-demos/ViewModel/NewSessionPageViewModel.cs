using System;
using System.ComponentModel;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class NewSessionPageViewModel : ViewModelBase
    {
        private int sessionCode;

        public NewSessionPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.SessionCode = sessionCode;
        }

        public int SessionCode
        {
            get
            {
                return this.sessionCode;
            }

            set
            {
                this.sessionCode = value;
                this.RaisePropertyChanged("SessionCode");
            }
        }

        public ICommand OpenExplorerCommand
        {
            get
            {
                return new DelegateCommand(this.ConnectToSession);
            }
        }

        private void ConnectToSession(object arg)
        {
            this.BusyMessage = "Logging in...";
            this.IsBusy = true;
            App.CurrentUser = User.A;
            Mobeelizer.Login(SessionCode.ToString(), Resources.Config.c_userALogin, Resources.Config.c_userAPassword, (error) =>
            {
                this.IsBusy = false;
                try
                {
                    if (error == null)
                    {
                        App.CurrentUser = User.A;
                        PushNotificationService.Instance.PerformUserRegistration();
                        navigationService.Navigate(new Uri(String.Format("/View/ExplorePage.xaml?SessionCode={0}", this.SessionCode), UriKind.Relative));
                    }
                    else if (error.Code == "missingConnection")
                    {
                        this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_missingConnection);
                    }
                    else
                    {
                        this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                    }
                }
                catch
                {
                    this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                }
            });
        }
    }
}
