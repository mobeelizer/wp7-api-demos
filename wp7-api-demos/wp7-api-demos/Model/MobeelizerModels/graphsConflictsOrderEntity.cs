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
        public override string guid { get; set; }

        [Column()]
        public String name { get; set; }

        [Column()]
        public int status { get; set; }

        public ObservableCollection<graphsConflictsItemEntity> Items { get; set; }

        //public bool Conflicted
        //{
        //    get
        //    {
        //        return base.conflicted;
        //    }
        //}

        //public String Owner
        //{
        //    get
        //    {
        //        return base.owner;
        //    }
        //}

        public ICommand AddCommand { get; set; }
    }
}
