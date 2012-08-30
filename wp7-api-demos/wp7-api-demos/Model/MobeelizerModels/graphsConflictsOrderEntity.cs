using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace wp7_api_demos.Model.MobeelizerModels
{
    [Table]
    public class graphsConflictsOrderEntity : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey = true)]
        public override string Guid { get; set; }

        [Column()]
        public override String Owner { get; set; }

        [Column()]
        public override bool Conflicted { get; set; }

        [Column()]
        public override bool Deleted { get; set; }

        [Column()]
        public override bool Modified { get; set; }

        [Column()]
        public String Name { get; set; }

        [Column()]
        public int Status { get; set; }

        public ObservableCollection<graphsConflictsItemEntity> Items { get; set; }

        public ICommand AddCommand { get; set; }
    }
}
