﻿using System;
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
    }
}