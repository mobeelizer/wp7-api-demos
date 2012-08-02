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
    public class SelectScorePageViewModel : ViewModelBase
    {
        private String modelGuid;

        public ObservableCollection<int> Options { get; private set; }

        public SelectScorePageViewModel(INavigationService navigationService, String modelGuid)
            : base(navigationService)
        {
            this.modelGuid = modelGuid;
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
                    var query = from conflictsEntity e in transaction.GetModels<conflictsEntity>() where e.guid == modelGuid select e;
                    conflictsEntity entity = query.Single();
                    entity.score = value;
                    transaction.Commit();
                }
                this.navigationService.GoBack();
            }
        }
    }
}
