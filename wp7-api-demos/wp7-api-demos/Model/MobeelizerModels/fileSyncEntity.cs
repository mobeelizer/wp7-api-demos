using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.Model.MobeelizerModels
{
    [Table]
    public class fileSyncEntity : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey= true)]
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
        public String Photo { get; set; }

        public IMobeelizerFile PhotoFile
        {
            get
            {
                return base.GetFile(Photo);
            }

            set
            {
                this.Photo = base.SetFile(value);
            }
        }
    }
}

