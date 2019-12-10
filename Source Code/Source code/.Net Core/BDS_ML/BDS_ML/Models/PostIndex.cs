using BDS_ML.Models.ModelDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models
{
    public class PostIndex
    {
        private readonly BDT_MLDBContext db;

        public PostIndex()
        {
            db = new BDT_MLDBContext();
        }

        public PostIndex(BDT_MLDBContext db)
        {
            this.db = db;
        }

        public List<Post> get3HotPosts()
        {
            List<Post> lst3HotPosts = db.Post.Include(p => p.Post_Status)
              .ThenInclude(post => post.StatusNavigation.Post_Status)
              .Include(p=>p.Post_Location).ThenInclude(l=>l.Tinh_TPNavigation.Post_Location)
              .Include(p=>p.Post_Location).ThenInclude(l=>l.Quan_HuyenNavigation.Post_Location)
              .Include(i=>i.Post_Image)
              .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1)
              .OrderByDescending(n => n.PostTime).Take(3).ToList();

            return lst3HotPosts;
        }

        public List<Post> get6PopularPosts()
        {
            List<Post> lst6PopularPosts = db.Post.Include(p => p.Post_Status)
              .ThenInclude(post => post.StatusNavigation.Post_Status)
              .Include(p => p.Post_Location).ThenInclude(l => l.Tinh_TPNavigation.Post_Location)
              .Include(p => p.Post_Location).ThenInclude(l => l.Quan_HuyenNavigation.Post_Location)
              .Include(i => i.Post_Image)
              .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1)
              .OrderByDescending(n => n.PostTime).Take(6).ToList();

            return lst6PopularPosts;
        }

        public int getCount(int id)
        {
            int result = 0;
            result = db.Post.Include(p => p.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == id).Count();
            return result;
        }
    }
}
