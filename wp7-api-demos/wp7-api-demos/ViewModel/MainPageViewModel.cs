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
        private const String SESSION_KEY = "CurrentSession";

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(SESSION_KEY))
            {
                int? sessionCode = (int?)settings[SESSION_KEY];
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

        public int SessionCode { get; set; }

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
                        this.SaveSessionCode(this.SessionCode);
                        this.navigationService.Navigate(new Uri(String.Format("/View/NewSessionPage.xaml?SessionCode={0}", this.SessionCode), UriKind.Relative));
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
                                PushNotificationService.Instance.PerformUserRegistration();
                                this.SaveSessionCode(this.SessionCode);
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

        private void SaveSessionCode(int sessionCode)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains(SESSION_KEY))
            {
                settings.Add(SESSION_KEY, this.SessionCode);
            }
            else
            {
                settings[SESSION_KEY] = this.SessionCode;
            }

            settings.Save();
        }

    }
}
