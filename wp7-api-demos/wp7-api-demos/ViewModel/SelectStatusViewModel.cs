using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Com.Mobeelizer.Mobile.Wp7;
using wp7_api_demos.Model.MobeelizerModels;
using System.Linq;

namespace wp7_api_demos.ViewModel
{
    public class SelectStatusViewModel : ViewModelBase
    {
        private String modelGuid;

        public String Title { get; set; }

        public ObservableCollection<int> Options { get; private set; }

        public SelectStatusViewModel(INavigationService navigationService, String modelGuid)
            : base(navigationService)
        {
            this.modelGuid = modelGuid;
            using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var query = from graphsConflictsOrderEntity e in transaction.GetModels<graphsConflictsOrderEntity>() where e.guid == modelGuid select e;
                graphsConflictsOrderEntity entity = query.Single();
                this.Title = entity.name;
            }
            this.Options = new ObservableCollection<int>();
            for(int i = 1; i<6; ++i)
            {
                this.Options.Add(i);
            }
        }

        public int SelectedOption
        {
            get
            {
                return -1;
            }

            set
            {
                using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
                {
                    var query = from graphsConflictsOrderEntity e in transaction.GetModels<graphsConflictsOrderEntity>() where e.guid == modelGuid select e;
                    graphsConflictsOrderEntity entity = query.Single();
                    entity.status = value;
                    transaction.Commit();
                }
                this.navigationService.GoBack();
            }
        }
    }
}
