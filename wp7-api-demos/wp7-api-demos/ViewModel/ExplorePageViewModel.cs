using System;
using System.Windows.Input;
using System.Collections.Generic;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class ExplorePageViewModel : ViewModelBase
    {
        private int sessionCode;

        private ICommand yourSessionCommand;

        public ICommand YourSessionCommand
        {
            get
            {
                if (this.yourSessionCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.yourSessionCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/SessionInfoPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.yourSessionCommand;
            }
        }

        private ICommand aboutPageCommand;

        public ICommand AboutPageCommand
        {
            get
            {
                if (this.aboutPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.aboutPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/AboutAsPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.aboutPageCommand;
            }
        }

        private ICommand simpleSyncPageCommand;

        public ICommand SimpleSyncPageCommand
        {
            get
            {
                if (this.simpleSyncPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.simpleSyncPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/SimpleSyncPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.simpleSyncPageCommand;
            }
        }

        private ICommand filesPageCommand;

        public ICommand FilesPageCommand
        {
            get
            {
                if (this.filesPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.filesPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/FilesPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.filesPageCommand;
            }
        }

        private ICommand permisionsPageCommand;

        public ICommand PermisionsPageCommand
        {
            get
            {
                if (this.permisionsPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.permisionsPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/PermisionsPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.permisionsPageCommand;
            }
        }

        private ICommand conflictsPageCommand;

        public ICommand ConflictsPageCommand
        {
            get
            {
                if (this.conflictsPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.conflictsPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/ConflictsPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.conflictsPageCommand;
            }
        }

        private ICommand relationConflictsPageCommand;

        public ICommand RelationConflictsPageCommand
        {
            get
            {
                if (this.relationConflictsPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.relationConflictsPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/RelationConflictsPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.relationConflictsPageCommand;
            }
        }

        private ICommand pushNotificationPageCommand;

        public ICommand PushNotificationPageCommand
        {
            get
            {
                if (this.pushNotificationPageCommand == null)
                {
                    String sessionParam = String.Format("?SessionCode={0}", sessionCode);
                    this.pushNotificationPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/PushNotificationPage.xaml" + sessionParam, UriKind.Relative)); });
                }

                return this.pushNotificationPageCommand;
            }
        }

        private ICommand whatNextPageCommand;

        public ICommand WhatNextPageCommand
        {
            get
            {
                if (this.whatNextPageCommand == null)
                {
                    this.whatNextPageCommand = new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/WhatNextPage.xaml", UriKind.Relative)); });
                }

                return this.whatNextPageCommand;
            }
        }

        public ExplorePageViewModel(INavigationService navigationService, int sessionCode)
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
            this.IsBusy = false;
        }

        private void SwitchingUser(object arg)
        {
            this.BusyMessage = "Logging in";
            this.IsBusy = true;
        }
    }
}
