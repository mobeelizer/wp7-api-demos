using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
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
        }

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
            // TODO: synchronize
            this.RefreshEntitiesList();
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
