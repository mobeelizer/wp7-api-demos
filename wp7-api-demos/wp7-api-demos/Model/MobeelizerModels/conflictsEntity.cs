using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Data.Linq.Mapping;

namespace wp7_api_demos.Model.MobeelizerModels
{
    [Table]
    public class conflictsEntity : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey = true)]
        public override string guid { get; set; }

        [Column()]
        public int score { get; set; }

        [Column()]
        public String title { get; set; }

        public String Owner
        {
            get
            {
                return base.owner;
            }
        }

        public bool Conflicted
        {
            get
            {
                return base.conflicted;
            }
        }
    }
}
