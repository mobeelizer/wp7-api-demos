using System;
using System.ComponentModel;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class NewSessionPageViewModel : INotifyPropertyChanged
    {
        private INavigationService navigationService;

        private int sessionCode;

        private bool isBusy;

        private String busyMessage;

        public NewSessionPageViewModel(INavigationService navigationService, int sessionCode)
        {
            this.navigationService = navigationService;
            this.SessionCode = sessionCode;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void ConnectToSession(object arg)
        {
            this.BusyMessage = "Logging in...";
            this.IsBusy = true;
            Mobeelizer.Login(SessionCode.ToString(), Resources.Config.c_userALogin, Resources.Config.c_userAPassword, (result) =>
            {
                this.IsBusy = false;
                try
                {
                    var status = result.GetLoginStatus();
                    switch (status)
                    {
                        case MobeelizerLoginStatus.OK:
                            navigationService.Navigate(new Uri(String.Format("/View/ExplorePage.xaml?SessionCode={0}", this.SessionCode), UriKind.Relative));
                            break;
                        case MobeelizerLoginStatus.MISSING_CONNECTION_FAILURE:
                            this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_missingConnection);
                            break;
                        case MobeelizerLoginStatus.CONNECTION_FAILURE:
                        case MobeelizerLoginStatus.AUTHENTICATION_FAILURE:
                        case MobeelizerLoginStatus.OTHER_FAILURE:
                            this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                            break;
                    }
                }
                catch
                {
                    this.navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                }
            });
        }

        private void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
