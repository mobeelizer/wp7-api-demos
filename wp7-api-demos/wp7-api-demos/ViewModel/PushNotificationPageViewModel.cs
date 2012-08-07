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
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7;
using System.Collections.Generic;

namespace wp7_api_demos.ViewModel
{
    public class PushNotificationPageViewModel : ViewModelBase
    {
        public PushNotificationPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.SessionCode = sessionCode;
        }

        public int SessionCode { get; private set; }

        public ICommand SendPushCommand
        {
            get
            {
                return new DelegateCommand(this.OnSendPush);
            }
        }

        public ICommand SendPushToACommand
        {
            get
            {
                return new DelegateCommand(this.OnSendToA);
            }
        }

        public ICommand SendPushToBCommand
        {
            get
            {
                return new DelegateCommand(this.OnSendToB);
            }
        }

        public ICommand UserSwitchedCommand
        {
            get
            {
                return new DelegateCommand(UserSwitched);
            }
        }

        public ICommand SwitchingUserCommand
        {
            get
            {
                return new DelegateCommand(SwitchingUser);
            }
        }

        private void UserSwitched(object arg)
        {
            MobeelizerLoginStatus status = (MobeelizerLoginStatus)arg;
            switch (status)
            {
                case MobeelizerLoginStatus.OK:
                    this.IsBusy = false;
                    break;
                case MobeelizerLoginStatus.MISSING_CONNECTION_FAILURE:
                    navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_missingConnection);
                    break;
                default:
                case MobeelizerLoginStatus.AUTHENTICATION_FAILURE:
                case MobeelizerLoginStatus.CONNECTION_FAILURE:
                case MobeelizerLoginStatus.OTHER_FAILURE:
                    navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
                    break;
            }
        }

        private void SwitchingUser(object arg)
        {
            this.BusyMessage = "Logging in";
            this.IsBusy = true;
        }

        private void OnSendPush(object args)
        {
            this.BusyMessage = "Sending push notification";
            this.IsBusy = true;
            IDictionary<String, String> notification = new Dictionary<String, String>();
            notification.Add("X-NotificationClass", "2");
            notification.Add("X-WindowsPhone-Target", "toast");
            notification.Add("Text1", "Push received!");
            notification.Add("Text2", "Wp7 device greets all users.");
            notification.Add("Param", "/View/MainPage.xaml");
            notification.Add("alert", "Wp7 device greets all users.");
            Mobeelizer.SendRemoteNotification(notification, (result) =>
                {
                    this.IsBusy = false;
                    if (result.GetCommunicationStatus() == MobeelizerCommunicationStatus.FAILURE)
                    {
                        this.navigationService.ShowMessage(Resources.Errors.e_title, "Unable to send push notification.");
                    }
                });
        }

        private void OnSendToA(object args)
        {
            this.BusyMessage = "Sending push notification";
            this.IsBusy = true;
            IDictionary<String, String> notification = new Dictionary<String, String>();
            notification.Add("X-NotificationClass", "2");
            notification.Add("X-WindowsPhone-Target", "toast");
            notification.Add("Text1", "Push received!");
            notification.Add("Text2", "Wp7 device greets user A.");
            notification.Add("Param", "/View/MainPage.xaml");
            notification.Add("alert", "Wp7 device greets user A.");
            IList<String> users = new List<String>();
            users.Add("A");
            
            Mobeelizer.SendRemoteNotificationToUsers(notification,users, (result) =>
            {
                this.IsBusy = false;
                if (result.GetCommunicationStatus() == MobeelizerCommunicationStatus.FAILURE)
                {
                    this.navigationService.ShowMessage(Resources.Errors.e_title, "Unable to send push notification.");
                }
            });
        }

        private void OnSendToB(object args)
        {
            this.BusyMessage = "Sending push notification";
            this.IsBusy = true;
            IDictionary<String, String> notification = new Dictionary<String, String>();
            notification.Add("X-NotificationClass", "2");
            notification.Add("X-WindowsPhone-Target", "toast");
            notification.Add("Text1", "Push received!");
            notification.Add("Text2", "Wp7 device greets user B.");
            notification.Add("Param", "/View/MainPage.xaml");
            notification.Add("alert", "Wp7 device greets user B.");
            IList<String> users = new List<String>();
            users.Add("B");
            Mobeelizer.SendRemoteNotificationToUsers(notification, users, (result) =>
            {
                this.IsBusy = false;
                if (result.GetCommunicationStatus() == MobeelizerCommunicationStatus.FAILURE)
                {
                    this.navigationService.ShowMessage(Resources.Errors.e_title, "Unable to send push notification.");
                }
            });
        }
    }
}
