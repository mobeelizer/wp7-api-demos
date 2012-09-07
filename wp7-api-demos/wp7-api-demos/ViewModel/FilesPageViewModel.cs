using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using wp7_api_demos.Model.MobeelizerModels;
using wp7_api_demos.Model;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7;
using System.Linq;
using System.Windows.Resources;

namespace wp7_api_demos.ViewModel
{
    public class FilesPageViewModel : ViewModelBase
    {
        public ObservableCollection<fileSyncEntity> Entities { get; set; }

        public FilesPageViewModel(IFilesPageNavigationService navigationService, int sessionCode) 
            : base(navigationService)
        {
            this.Entities = new ObservableCollection<fileSyncEntity>();
            this.RefreshEntitiesList();
            this.SessionCode = sessionCode;
        }

        public fileSyncEntity SelectedPhoto
        {
            get
            {
                return null;
            }

            set
            {
                if (value != null)
                {
                    (this.navigationService as IFilesPageNavigationService).ShowPhoto(value.PhotoFile);
                    this.RaisePropertyChanged("SelectedPhoto");
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

        private IMobeelizerFile GetRandomPhoto()
        {
            String uri = String.Format("Resources/images/{0}", DataUtil.GetRandomPhoto());
            StreamResourceInfo info = Application.GetResourceStream(new Uri(uri, UriKind.Relative));
            return Mobeelizer.CreateFile("photo", info.Stream); ;
        }

        private void OnAdd(object param)
        {
            (this.navigationService as IFilesPageNavigationService).GetPhoto((photo)=>
            {
                fileSyncEntity entity = new fileSyncEntity();
                if (photo == null)
                {
                    photo = this.GetRandomPhoto();
                }

                entity.PhotoFile = photo;
                using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
                {
                    transaction.GetModelSet<fileSyncEntity>().InsertOnSubmit(entity);
                    transaction.SubmitChanges();
                }

                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Entities.Add(entity);
                }));
            });

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
                var query = from fileSyncEntity entity in transaction.GetModelSet<fileSyncEntity>() select entity;
                foreach (var entity in query)
                {
                    this.Entities.Add(entity);
                }
            }
        }
    }
}
