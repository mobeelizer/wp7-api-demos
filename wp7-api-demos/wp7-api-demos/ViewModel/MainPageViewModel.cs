using System;
using System.Windows.Input;
using wp7_api_demos.Model;
using System.ComponentModel;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Windows;
using System.IO.IsolatedStorage;

namespace wp7_api_demos.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            if (SessionSettings.IsSessionCreated)
            {
                int? sessionCode = SessionSettings.GetSessionCode();
                if (sessionCode != null)
                {
                    this.SessionCode = sessionCode.Value.ToString();
                    this.ConnectToExistingSession();
                }
            }
        }

        public ICommand CreateNewSessionCommand
        {
            get
            {
                return new DelegateCommand(this.CreateNewSession);
            }
        }

        private DelegateCommand connectToSessionCommand;

        public DelegateCommand ConnectToSessionCommand
        {
            get
            {
                if (this.connectToSessionCommand == null)
                {
                    this.connectToSessionCommand = new DelegateCommand(this.ConnectToExistingSession, (arg) => { return !String.IsNullOrWhiteSpace(this.SessionCode); });
                }

                return this.connectToSessionCommand;
            }
        }

        private String sessionCode;

        public String SessionCode
        {
            get
            {
                return this.sessionCode;
            }

            set
            {
                int intValue;
                if (Int32.TryParse(value, out intValue))
                {
                    this.sessionCode = value;
                }
                else
                {
                    this.sessionCode = null;
                }
                
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.RaisePropertyChanged("SessionCode");
                        ConnectToSessionCommand.RaiseCanExecuteChanged();
                    }));
            }
        }

        private void CreateNewSession(object arg)
        {
            PageSettings.ResetSettings();
            this.BusyMessage = "Creating new demo session...";
            this.IsBusy = true;

            CreateSessionTask createTask = new CreateSessionTask((sessionCode) =>
                {
                    this.IsBusy = false;
                    if (sessionCode == null)
                    {
                        navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotCreateSession);
                    }
                    else
                    {
                        this.SessionCode = sessionCode;
                        SessionSettings.SaveSessionCode(Int32.Parse(this.SessionCode));
                        navigationService.Navigate(new Uri(String.Format("/View/NewSessionPage.xaml?SessionCode={0}", this.SessionCode), UriKind.Relative));
                    }
                });
            createTask.Execute();
        }

        private void ConnectToExistingSession()
        {
            this.BusyMessage = "Logging in...";
            this.IsBusy = true;
            App.CurrentUser = User.A;
            Mobeelizer.Login(SessionCode.ToString(), Resources.Config.c_userALogin, Resources.Config.c_userAPassword, (result) =>
            {
                this.IsBusy = false;
                try
                {
                    var status = result.GetLoginStatus();
                    switch (status)
                    {
                        case MobeelizerLoginStatus.OK:
                            App.CurrentUser = User.A;
                            SessionSettings.SaveSessionCode(Int32.Parse(this.SessionCode));
                            PushNotificationService.Instance.PerformUserRegistration();
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

        private void ConnectToExistingSession(object arg)
        {
            PageSettings.ResetSettings();
            this.ConnectToExistingSession();
        }
    }
}
