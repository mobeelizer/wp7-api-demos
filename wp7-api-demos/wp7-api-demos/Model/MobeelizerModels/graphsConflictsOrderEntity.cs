using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;

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
    }
}
