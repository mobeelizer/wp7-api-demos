using System;
using System.Collections.ObjectModel;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_api_demos.Model.MobeelizerModels;
using System.Windows.Input;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class SelectScorePageViewModel : ViewModelBase
    {
        private String modelGuid;

        public String Title { get; set; }

        public ObservableCollection<ListOption> Options { get; private set; }

        public SelectScorePageViewModel(INavigationService navigationService, String modelGuid)
            : base(navigationService)
        {
            ICommand selectCommand = new DelegateCommand(ScoreSelected);
            this.modelGuid = modelGuid;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var query = from conflictsEntity e in transaction.GetModelSet<conflictsEntity>() where e.Guid == modelGuid select e;
                conflictsEntity entity = query.Single();
                this.Title = entity.Title;
            }

            this.Options = new ObservableCollection<ListOption>();
            for(int i = 1; i<6; ++i)
            {
                this.Options.Add(new ListOption() { Score = i, Command = selectCommand });
            }
        }

        

        private void ScoreSelected(object arg)
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
                    var query = from conflictsEntity e in transaction.GetModelSet<conflictsEntity>() where e.Guid == modelGuid select e;
                    conflictsEntity entity = query.Single();

                    entity.Score = value;
                    transaction.SubmitChanges();
                }

                this.navigationService.GoBack();
            }
        }

        public class ListOption
        {
            public ICommand Command { get; set; }

            public int Score { get; set; }
        }
    }

}
