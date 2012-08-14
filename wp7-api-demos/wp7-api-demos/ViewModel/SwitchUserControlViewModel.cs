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

        public User CurrentUser
        {
            get
            {
                return App.CurrentUser;
            }

            set
            {
                if (App.CurrentUser != value && !Mobeelizer.CheckSyncStatus().IsRunning())
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
                    Mobeelizer.Login(SessionCode.ToString(), user, password, (result) =>
                        {
                            if (result.GetLoginStatus() == Com.Mobeelizer.Mobile.Wp7.Api.MobeelizerLoginStatus.OK)
                            {
                                App.CurrentUser = value;
                                PushNotificationService.Instance.PerformUserRegistration();
                                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    this.RaisePropertyChanged("CurrentUser");
                                }));
                            }

                            this.UserSwitchedCommand.Execute(result.GetLoginStatus());
                        });
                }
                else
                {
                    this.RaisePropertyChanged("CurrentUser");
                }
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
    }
}
