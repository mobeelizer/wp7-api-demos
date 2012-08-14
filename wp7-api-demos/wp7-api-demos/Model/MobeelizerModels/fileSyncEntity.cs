using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.Model.MobeelizerModels
{
    [Table]
    public class fileSyncEntity : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey= true)]
        public override string guid { get; set; }

        [Column()]
        public String photo { get; set; }

        public IMobeelizerFile PhotoFile
        {
            get
            {
                return base.GetFile(photo);
            }

            set
            {
                this.photo = base.SetFile(value);
            }
        }

        //public String Owner
        //{
        //    get
        //    {
        //        return base.owner;
        //    }
        //}
    }
}

