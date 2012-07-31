﻿using System;
using System.Windows.Input;
using System.Collections.Generic;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class ExplorePageViewModel : ViewModelBase
    {
        private int sessionCode;

        public ExplorePageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.SessionCode = sessionCode;
            this.MenuItems = new Dictionary<String, ICommand>();
            String sessionParam = String.Format("?SessionCode={0}", sessionCode);
            this.MenuItems.Add(Resources.ResourceDictionary.m_about, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/AboutAsPage.xaml", UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_sync, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/SimpleSyncPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_files, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/FilesPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_permisions, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/PermisionsPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_conflicts, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/ConflictsPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_relations, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/RelationConflictsPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_push, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/PushNotificationPage.xaml" + sessionParam, UriKind.Relative)); }));
            this.MenuItems.Add(Resources.ResourceDictionary.m_whatNext, new DelegateCommand((o) => { this.navigationService.Navigate(new Uri("/View/WhatNextPage.xaml", UriKind.Relative)); }));
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

        public IDictionary<String, ICommand> MenuItems { get; private set; }
    }
}