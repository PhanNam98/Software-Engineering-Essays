using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Data;
using BDS_ML.Models.ModelDB;
using Microsoft.EntityFrameworkCore;

namespace BDS_ML.Respository
{
    public class PostRespository : IPostRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int getCount(int id)
        {
            int result = 0;
            result = DataProvider.Ins.db.Post.Include(p => p.Post_Status).Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1 && p.PostType == id).Count();
            return result;

        }

        public List<Post> GetPostByCond(int cond)
        {
            List<Post> lst3HotPosts = DataProvider.Ins.db.Post.Include(p => p.Post_Status)
            .ThenInclude(post => post.StatusNavigation.Post_Status)
            .Include(p => p.Post_Location).ThenInclude(l => l.Tinh_TPNavigation.Post_Location)
            .Include(p => p.Post_Location).ThenInclude(l => l.Quan_HuyenNavigation.Post_Location)
            .Include(i => i.Post_Image)
            .Include(p => p.Post_Detail)
            .Where(p => p.Post_Status.OrderBy(c => c.ModifiedDate).LastOrDefault().Status == 1)
            .OrderByDescending(n => n.PostTime).Take(cond).ToList();
            return lst3HotPosts;

        }

        public Post GetPostByID(int postId)
        {

            return DataProvider.Ins.db.Post.Include(p => p.ID_AccountNavigation).Include(p => p.PostTypeNavigation).Include(p => p.ProjectNavigation)
                                      .Include(p => p.RealEstateTypeNavigation).Include(p => p.Post_Location).ThenInclude(lo => lo.Tinh_TPNavigation.Post_Location)
                                      .ThenInclude(lo => lo.Quan_HuyenNavigation.Post_Location).ThenInclude(lo => lo.Phuong_XaNavigation.Post_Location)
                                      .ThenInclude(lo => lo.Duong_PhoNavigation.Post_Location)
                                      .Include(d => d.Post_Detail).
                                      Include(image => image.Post_Image)
                                      .Include(p => p.Post_Status)
                                      .ThenInclude(pt => pt.StatusNavigation.Post_Status).Where(p => p.ID_Post == postId).SingleOrDefault();

        }

        public void InsertPost(Post post)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
