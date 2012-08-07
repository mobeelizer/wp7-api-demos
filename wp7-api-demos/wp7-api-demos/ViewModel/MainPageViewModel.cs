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
                    this.SessionCode = sessionCode.Value;
                    this.ConnectToSessionCommand.Execute(null);
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

        public ICommand ConnectToSessionCommand
        {
            get
            {
                return new DelegateCommand(this.ConnectToExistingSession);
            }
        }

        private int sessionCode;

        public int SessionCode
        {
            get
            {
                return this.sessionCode;
            }

            set
            {
                this.sessionCode = value;
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.RaisePropertyChanged("SessionCode");
                    }));
            }
        }

        private void CreateNewSession(object arg)
        {
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
                        this.SessionCode = Int32.Parse(sessionCode);
                        SessionSettings.SaveSessionCode(this.SessionCode);
                        Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ConnectToExistingSession(null);
                            }));
                    }
                });
            createTask.Execute();
        }

        private void ConnectToExistingSession(object arg)
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
                                SessionSettings.SaveSessionCode(this.SessionCode);
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
    }
}
