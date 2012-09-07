using System;
using System.Windows.Input;
using System.ComponentModel;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7;
using System.Windows;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.ViewModel
{
    public class SwitchUserControlViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchingUserCommand { private get; set; }

        public ICommand UserSwitchedCommand { private get; set; }

        public int SessionCode { private get; set; }

        public bool UserAEnabled
        {
            get
            {
                return (App.CurrentUser == User.A) ? false : true;
            }
        }

        public bool UserBEnabled
        {
            get
            {
                return (App.CurrentUser == User.A) ? true : false;
            }
        }

        private ICommand switchUserCommand;

        public ICommand SwitchUserCommand
        {
            get
            {
                if (this.switchUserCommand == null)
                {
                    this.switchUserCommand = new DelegateCommand(SwitchUser);
                }

                return this.switchUserCommand;
            }
        }

        private void SwitchUser(object arg)
        {
            User value = (App.CurrentUser == User.A) ? User.B : User.A;
            if (!Mobeelizer.CheckSyncStatus().IsRunning())
            {
                this.SwitchingUserCommand.Execute(null);
                String user = String.Empty;
                String password = String.Empty;
                switch (value)
                {
                    case User.A:
                        user = Resources.Config.c_userALogin;
                        password = Resources.Config.c_userAPassword;
                        break;
                    case User.B:
                        user = Resources.Config.c_userBLogin;
                        password = Resources.Config.c_userBPassword;
                        break;
                }
                Mobeelizer.UnregisterForRemoteNotifications(e =>
                    {
                        Mobeelizer.Login(SessionCode.ToString(), user, password, (error) =>
                        {
                            if (error == null)
                            {
                                App.CurrentUser = value;
                                PushNotificationService.Instance.PerformUserRegistration();
                                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    this.RaisePropertyChanged("UserAEnabled");
                                    this.RaisePropertyChanged("UserBEnabled");
                                }));
                            }

                            this.UserSwitchedCommand.Execute(error);
                        });
                    });
            }
            else
            {
                this.RaisePropertyChanged("UserAEnabled");
                this.RaisePropertyChanged("UserBEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void Refresh()
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.RaisePropertyChanged("UserAEnabled");
                this.RaisePropertyChanged("UserBEnabled");
            }));
        }
    }
}
