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
using System.Windows.Data;
using System.ComponentModel;


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
            this.EntitiesViewSource = new CollectionViewSource();
            this.Entities = new ObservableCollection<conflictsEntity>();
            this.EntitiesViewSource.Source = this.Entities;
            this.EntitiesViewSource.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending)); 
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
                        this.navigationService.Navigate(new Uri(String.Format("/View/SelectScorePage.xaml?guid={0}", this.selectedEntity.Guid), UriKind.Relative));
                    }
                }
            }
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
            MobeelizerOperationError error = (MobeelizerOperationError)arg;
            if (error == null)
            {
                this.IsBusy = false;
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.RefreshEntitiesList();
                }));
            }
            else if (error.Code == "missingConnection")
            {
                navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_missingConnection);
            }
            else
            {
                navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_cannotConnectToSession);
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
            entity.Title = movie.Title;
            entity.Score = movie.Rating;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModelSet<conflictsEntity>().InsertOnSubmit(entity);
                transaction.SubmitChanges();
            }

            this.Entities.Add(entity);
        }

        private void OnSync(object param)
        {
            this.BusyMessage = "Synchronizing";
            this.IsBusy = true;
            Mobeelizer.Sync((error) =>
            {
                this.IsBusy = false;
                if (error == null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.RefreshEntitiesList();
                    }));
                }
                else
                {
                    navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_syncFailed);
                }
            });
        }

        private void RefreshEntitiesList()
        {
            this.Entities.Clear();
            bool inConflict = false;
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                var query = from conflictsEntity entity in transaction.GetModelSet<conflictsEntity>() select entity;
                foreach (var entity in query)
                {
                    if (entity.Conflicted)
                    {
                        inConflict = true;
                    }

                    Entities.Add(entity);
                }
            }
            
            if (IsWarningVisable != inConflict)
            {
                IsWarningVisable = inConflict;
            }
        }

        public CollectionViewSource EntitiesViewSource { get; set; }
    }
}
