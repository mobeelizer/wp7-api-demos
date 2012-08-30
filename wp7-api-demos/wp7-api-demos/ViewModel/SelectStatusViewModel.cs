using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model;
using wp7_api_demos.Model.MobeelizerModels;

namespace wp7_api_demos.ViewModel
{
    public class SelectStatusViewModel : ViewModelBase
    {
        private String modelGuid;

        public String Title { get; set; }

        public ObservableCollection<ListOption> Options { get; private set; }

        public SelectStatusViewModel(INavigationService navigationService, String modelGuid)
            : base(navigationService)
        {

            ICommand selectCommand = new DelegateCommand(StatusSelected);
            this.modelGuid = modelGuid;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var query = from graphsConflictsOrderEntity e in transaction.GetModelSet<graphsConflictsOrderEntity>() where e.Guid == modelGuid select e;
                graphsConflictsOrderEntity entity = query.Single();
                this.Title = entity.Name;
            }
            this.Options = new ObservableCollection<ListOption>();
            for(int i = 1; i<6; ++i)
            {
                this.Options.Add(new ListOption() { Status = i, Command = selectCommand });
            }
        }


        private void StatusSelected(object arg)
        {
            int value = (int)arg;
            if (Mobeelizer.CheckSyncStatus().IsRunning())
            {
                navigationService.ShowMessage(Resources.Errors.e_title, Resources.Errors.e_waitUntilSyncFinish);
            }
            else
            {
                using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
                {
                    var query = from graphsConflictsOrderEntity e in transaction.GetModelSet<graphsConflictsOrderEntity>() where e.Guid == modelGuid select e;
                    graphsConflictsOrderEntity entity = query.Single();
                    entity.Status = value;
                    transaction.SubmitChanges();
                }
                this.navigationService.GoBack();
            }
        }

        public class ListOption
        {
            public ICommand Command { get; set; }

            public int Status { get; set; }
        }
    }
}
