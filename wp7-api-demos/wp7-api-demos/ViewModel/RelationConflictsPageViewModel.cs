using System;
using System.Windows;
using System.Windows.Input;
using wp7_api_demos.Model.MobeelizerModels;
using System.Collections.ObjectModel;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7;
using System.Linq;

namespace wp7_api_demos.ViewModel
{
    public class RelationConflictsPageViewModel : ViewModelBase
    {
        private ObservableCollection<graphsConflictsOrderEntity> entities;

        public ObservableCollection<graphsConflictsOrderEntity> Entities
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

        public RelationConflictsPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<graphsConflictsOrderEntity>();
            this.RefreshEntitiesList();
            this.SessionCode = sessionCode;
        }

        private graphsConflictsOrderEntity selectedEntity;

        public graphsConflictsOrderEntity SelectedEntity
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
                        this.navigationService.Navigate(new Uri(String.Format("/View/SelectStatusPage.xaml?guid={0}", this.selectedEntity.guid), UriKind.Relative));
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

        private ICommand addItemCommand;

        public ICommand AddItemCommand
        {
            get
            {
                if (this.addItemCommand == null)
                {
                    this.addItemCommand = new DelegateCommand(this.OnAddRelation);
                }

                return this.addItemCommand;
            }
        }

        private ICommand removeItemCommand;

        public ICommand RemoveItemCommand
        {
            get
            {
                if (this.removeItemCommand == null)
                {
                    this.removeItemCommand = new DelegateCommand(this.OnRemoveRelation);
                }

                return this.removeItemCommand;
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
            graphsConflictsOrderEntity entity = new graphsConflictsOrderEntity();
            entity.name = this.GetName();
            entity.status = new Random().Next(1, 5);
            entity.AddCommand = AddItemCommand;
            entity.Items = new ObservableCollection<graphsConflictsItemEntity>();
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModelSet<graphsConflictsOrderEntity>().InsertOnSubmit(entity);
                transaction.SubmitChanges();
            }

            this.Entities.Add(entity);
        }

        private string GetName()
        {
            String user = App.CurrentUser.ToString();
            int number = new Random().Next(0, 99);
            return String.Format("{0}/00{1}", user, number);
        }

        private void OnAddRelation(object param)
        {
            graphsConflictsOrderEntity order = param as graphsConflictsOrderEntity;
            Movie movie = DataUtil.GetRandomMovie();
            graphsConflictsItemEntity entity = new graphsConflictsItemEntity();
            entity.title = movie.Title;
            entity.orderGuid = order.guid;
            entity.RemoveCommand = RemoveItemCommand;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                transaction.GetModelSet<graphsConflictsItemEntity>().InsertOnSubmit(entity);
                transaction.SubmitChanges();
            }
            if (order.Items == null)
            {
                order.Items = new ObservableCollection<graphsConflictsItemEntity>();
            }

            order.Items.Add(entity);
        }

        private void OnRemoveRelation(object param)
        {
            if (Mobeelizer.CheckSyncStatus().IsRunning())
            {
                navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_waitUntilSyncFinish);
            }
            else
            {
                graphsConflictsItemEntity item = param as graphsConflictsItemEntity;
                using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
                {
                    transaction.GetModelSet<graphsConflictsItemEntity>().DeleteOnSubmit(item);
                    transaction.SubmitChanges();
                }

                RefreshEntitiesList();
            }
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
            ObservableCollection<graphsConflictsOrderEntity> entities = new ObservableCollection<graphsConflictsOrderEntity>();
            var database = Mobeelizer.GetDatabase();
            using (var transaction = database.BeginTransaction())
            {
                var query = from graphsConflictsOrderEntity entity in transaction.GetModelSet<graphsConflictsOrderEntity>() select entity;
                foreach (var entity in query)
                {
                    entity.Items = new ObservableCollection<graphsConflictsItemEntity>();
                    var relationQuery = from graphsConflictsItemEntity r in transaction.GetModelSet<graphsConflictsItemEntity>() where r.orderGuid == entity.guid select r;
                    foreach (var relation in relationQuery)
                    {
                        if (relation.conflicted)
                        {
                            inConflict = true;
                        }
                        relation.RemoveCommand = RemoveItemCommand;
                        entity.Items.Add(relation);
                    }

                    if (entity.conflicted)
                    {
                        inConflict = true;
                    }

                    entity.AddCommand = AddItemCommand;
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
    }
}
