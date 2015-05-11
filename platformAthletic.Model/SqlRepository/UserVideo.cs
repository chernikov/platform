using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        

        public IQueryable<UserVideo> UserVideos
        {
            get
            {
                return Db.UserVideos;
            }
        }

        public bool CreateUserVideo(UserVideo instance)
        {
            if (instance.ID == 0)
            {
                Db.UserVideos.InsertOnSubmit(instance);
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserVideo(UserVideo instance)
        {
            var cache = Db.UserVideos.Where(p => p.ID == instance.ID).FirstOrDefault();
            if (cache != null)
            {
                cache.Header = instance.Header;
                cache.UserID = instance.UserID;
                cache.VideoUrl = instance.VideoUrl;
                cache.VideoCode = instance.VideoCode;
                cache.Preview = instance.Preview;
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserVideo(int idUserVideo)
        {
            var instance = Db.UserVideos.Where(p => p.ID == idUserVideo).FirstOrDefault();
            if (instance != null)
            {
                Db.UserVideos.DeleteOnSubmit(instance);
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        
    }
}