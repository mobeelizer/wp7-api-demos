using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;
using wp7_api_demos.Model.MobeelizerModels;
using System.Windows.Data;
using System.ComponentModel;

namespace wp7_api_demos.ViewModel
{
    public class PermisionsPageViewModel : ViewModelBase
    {
        public ObservableCollection<permissionsEntity> Entities { get; set; }

        public PermisionsPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<permissionsEntity>();
            this.EntitiesViewSorce = new CollectionViewSource();
            this.EntitiesViewSorce.Source = this.Entities;
            this.EntitiesViewSorce.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending)); 
            this.SessionCode = sessionCode;
            this.RefreshEntitiesList();
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

        private void OnAdd(object arg)
        {
            Movie movie = DataUtil.GetRandomMovie();
            permissionsEntity entity = new permissionsEntity();
            entity.Title = movie.Title;
            entity.Director = movie.Director;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModelSet<permissionsEntity>().InsertOnSubmit(entity);
                transaction.SubmitChanges();
            }

            this.Entities.Add(entity);
        }

        private void OnSync(object arg)
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
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                var query = from permissionsEntity entity in transaction.GetModelSet<permissionsEntity>() select entity;
                foreach (var entity in query)
                {
                    this.Entities.Add(entity);
                }
            }
        }

        public CollectionViewSource EntitiesViewSorce { get; set; }
    }
}
