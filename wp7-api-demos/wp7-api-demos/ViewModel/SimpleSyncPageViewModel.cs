using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;
using wp7_api_demos.Model.MobeelizerModels;
using System.Diagnostics;
using System.Windows.Data;
using System.ComponentModel;

namespace wp7_api_demos.ViewModel
{
    public class SimpleSyncPageViewModel : ViewModelBase
    {

        public ObservableCollection<simpleSyncEntity> Entities { get; set; }

        public CollectionViewSource EntitiesViewSorce { get; set; }

        public SimpleSyncPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<simpleSyncEntity>();
            this.EntitiesViewSorce = new CollectionViewSource();
            this.EntitiesViewSorce.Source = this.Entities;
            this.EntitiesViewSorce.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending)); 
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
            simpleSyncEntity entity = new simpleSyncEntity();
            entity.Title = movie.Title;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModelSet<simpleSyncEntity>().InsertOnSubmit(entity);
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
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                DateTime start = DateTime.Now;
                var query = from simpleSyncEntity entity in transaction.GetModelSet<simpleSyncEntity>() select entity;
                foreach (var entity in query)
                {
                    this.Entities.Add(entity);
                    String owner = entity.Owner;
                }
                DateTime end = DateTime.Now;
                TimeSpan difference = (end - start);
                Debug.WriteLine("Time is: {0} {1}", difference.Seconds, difference.Milliseconds); 
            }
        }
    }
}
