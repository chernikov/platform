using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Post> Posts
        {
            get
            {
                return Db.Posts;
            }
        }

        public bool CreatePost(Post instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = CurrentDateTime;
                instance.TitleImagePath = instance.TitleImagePath ?? "";
                Db.Posts.InsertOnSubmit(instance);
                Db.Posts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePost(Post instance)
        {
            var cache = Db.Posts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.Header = instance.Header;
				cache.Text = instance.Text;
                cache.TitleImagePath = instance.TitleImagePath;
                cache.Promoted = instance.Promoted;
                cache.IsVideo = instance.IsVideo;
                cache.VideoUrl = instance.VideoUrl;
                cache.VideoCode = instance.VideoCode;
                cache.VideoPreview = instance.VideoPreview;
                Db.Posts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ViewPost(int idPost)
        {
            var cache = Db.Posts.FirstOrDefault(p => p.ID == idPost);
            if (cache != null)
            {
                cache.CountOfView++;
                Db.Posts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePost(int idPost)
        {
            Post instance = Db.Posts.FirstOrDefault(p => p.ID == idPost);
            if (instance != null)
            {
                Db.Posts.DeleteOnSubmit(instance);
                Db.Posts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}