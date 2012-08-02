using System;
using System.Windows;
using System.Windows.Input;
using wp7_api_demos.Model.MobeelizerModels;
using System.Collections.ObjectModel;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7;
using System.Linq;
using System.Collections.Generic;


namespace wp7_api_demos.ViewModel
{
    public class ConflictsPageViewModel : ViewModelBase
    {
        private ObservableCollection<conflictsEntity> entities;

        public ObservableCollection<conflictsEntity> Entities
        {
            get
            {
                return this.entities;
            }

            set
            {
                this.entities = value;
                this.RaisePropertyChanged("Entities");
            }
        }

        public ConflictsPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<conflictsEntity>();
            this.RefreshEntitiesList();
            this.SessionCode = sessionCode;
        }

        private conflictsEntity selectedEntity;

        public conflictsEntity SelectedEntity
        {
            get
            {
                return selectedEntity;
            }

            set
            {
                if (this.selectedEntity != value)
                {
                    if (value != null)
                    {
                        this.selectedEntity = value;
                        this.navigationService.Navigate(new Uri(String.Format("/View/SelectScorePage.xaml?guid={0}", this.selectedEntity.guid), UriKind.Relative));
                    }
                }
            }
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
            conflictsEntity entity = new conflictsEntity();
            entity.title = movie.Title;
            entity.score = movie.Rating;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModels<conflictsEntity>().InsertOnSubmit(entity);
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
            bool inConflict = false;
            ObservableCollection<conflictsEntity> entities = new ObservableCollection<conflictsEntity>();
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                var query = from conflictsEntity entity in transaction.GetModels<conflictsEntity>() select entity;
                foreach (var entity in query)
                {
                    if (entity.Conflicted)
                    {
                        inConflict = true;
                    }

                    entities.Add(entity);
                }
            }
            
            if (IsWarningVisable != inConflict)
            {
                IsWarningVisable = inConflict;
            }

            this.Entities.Clear();
            this.Entities = entities;
        }

        private bool warningVisable;

        public bool IsWarningVisable
        {
            get
            {
                return this.warningVisable;
            }

            private set
            {
                this.warningVisable = value;
                this.RaisePropertyChanged("IsWarningVisable");
            }
        }
    }
}
