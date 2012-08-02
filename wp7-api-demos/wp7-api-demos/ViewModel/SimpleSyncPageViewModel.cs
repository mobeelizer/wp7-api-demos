﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;
using wp7_api_demos.Model.MobeelizerModels;

namespace wp7_api_demos.ViewModel
{
    public class SimpleSyncPageViewModel : ViewModelBase
    {
        public ObservableCollection<simpleSyncEntity> Entities { get; set; }

        public SimpleSyncPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<simpleSyncEntity>();
            this.RefreshEntitiesList();
            this.SessionCode = sessionCode;
        }

        public int SessionCode { get; private set; }

        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand(this.OnAdd);
            }
        }

        public ICommand SyncCommand
        {
            get
            {
                return new DelegateCommand(this.OnSync);
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
                    Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.RefreshEntitiesList();
                        }));
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

        private void OnAdd(object param)
        {
            Movie movie = DataUtil.GetRandomMovie();
            simpleSyncEntity entity = new simpleSyncEntity();
            entity.title = movie.Title;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModels<simpleSyncEntity>().InsertOnSubmit(entity);
                transaction.Commit();
            }

            this.Entities.Add(entity);
        }

        private void OnSync(object param)
        {
            this.BusyMessage = "Synchronizing";
            this.IsBusy = true;
            Mobeelizer.Sync((result) =>
            {
                this.IsBusy = false;
                switch (result.GetSyncStatus())
                {
                    case MobeelizerSyncStatus.FINISHED_WITH_SUCCESS:
                        Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.RefreshEntitiesList();
                            }));
                        break;
                    case MobeelizerSyncStatus.FINISHED_WITH_FAILURE:
                        navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_syncFailed);
                        break;
                    case MobeelizerSyncStatus.NONE:
                        navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_syncDisabled);
                        break;
                }
            });
        }

        private void RefreshEntitiesList()
        {
            this.Entities.Clear();
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                var query = from simpleSyncEntity entity in transaction.GetModels<simpleSyncEntity>() select entity;
                foreach (var entity in query)
                {
                    this.Entities.Add(entity);
                }
            }
        }
    }
}
