using System;
using System.Windows;
using Com.Mobeelizer.Mobile.Wp7;
using Microsoft.Phone.Notification;
using Coding4Fun.Phone.Controls;
using System.Xml.Linq;
using System.Linq;

namespace wp7_api_demos.Model
{
    public class PushNotificationService
    {
        private bool userARegistred;

        private bool userBRegistred;

        private String channelUri;

        private static PushNotificationService instance;


        private PushNotificationService()
        {
        }

        public static PushNotificationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PushNotificationService();
                }

                return instance;
            }
        }

        public void RegisterForRemoteNotification()
        {

            //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    MessageBox.Show("Register ");
            //}));

            HttpNotificationChannel pushChannel;
            string channelName = "wp7-api-demos-rawNotificationChannel";
            pushChannel = HttpNotificationChannel.Find(channelName);
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
                pushChannel.Open();
                pushChannel.BindToShellToast();

                //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    MessageBox.Show("new one ");
                //}));
            }
            else
            {
                //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    MessageBox.Show("already registred");
                //}));
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
                this.channelUri = pushChannel.ChannelUri.ToString();
            }
        }

        private  void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        MessageBox.Show("Channel updated " + e.ChannelUri);
            //    }));
            userARegistred = false;
            userBRegistred = false;
            channelUri = e.ChannelUri.ToString();
            Mobeelizer.RegisterForRemoteNotifications(channelUri, (result) =>
                {
                    if (result.GetCommunicationStatus() == Com.Mobeelizer.Mobile.Wp7.Api.MobeelizerCommunicationStatus.SUCCESS)
                    {
                        if (App.CurrentUser == User.A)
                            userARegistred = true;
                        else
                            userBRegistred = true;

                    }
                });
        }

        private void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    MessageBox.Show("Error: " + e.Message);
            //}));
            channelUri = null;
            Mobeelizer.UnregisterForRemoteNotifications((result) => { });
        }

        private void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ToastPrompt toast = new ToastPrompt();
                toast.Title = "Push received!";
                toast.Message = e.Collection["Text2"];
                toast.Show();
            }));
        }

        public void PerformUserRegistration()
        {
            //Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    MessageBox.Show("Perform user registration");
            //}));

            switch (App.CurrentUser)
            {
                case User.A:
                    if (!userARegistred && channelUri != null)
                    {
                        Mobeelizer.RegisterForRemoteNotifications(channelUri, (result) =>
                        {
                            if (result.GetCommunicationStatus() == Com.Mobeelizer.Mobile.Wp7.Api.MobeelizerCommunicationStatus.SUCCESS)
                            {
                                userARegistred = true;
                            }
                        });
                    }
                    break;
                case User.B:
                    if (!userBRegistred && channelUri != null)
                    {
                        Mobeelizer.RegisterForRemoteNotifications(channelUri, (result) =>
                        {
                            if (result.GetCommunicationStatus() == Com.Mobeelizer.Mobile.Wp7.Api.MobeelizerCommunicationStatus.SUCCESS)
                            {
                                userARegistred = true;
                            }
                        });
                    }
                    break;
            }
        }
    }
}
