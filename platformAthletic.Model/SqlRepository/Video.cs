using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Video> Videos
        {
            get
            {
                return Db.Videos;
            }
        }

        public bool CreateVideo(Video instance)
        {
            if (instance.ID == 0)
            {
                Db.Videos.InsertOnSubmit(instance);
                Db.Videos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateVideo(Video instance)
        {
            var cache = Db.Videos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TrainingID = instance.TrainingID;
				cache.Header = instance.Header;
				cache.Text = instance.Text;
				cache.VideoUrl = instance.VideoUrl;
				cache.VideoCode = instance.VideoCode;
                cache.Preview = instance.Preview;
                Db.Videos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveVideo(int idVideo)
        {
            Video instance = Db.Videos.FirstOrDefault(p => p.ID == idVideo);
            if (instance != null)
            {
                Db.Videos.DeleteOnSubmit(instance);
                Db.Videos.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}