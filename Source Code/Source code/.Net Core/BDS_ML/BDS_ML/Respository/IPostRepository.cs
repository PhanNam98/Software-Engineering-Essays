using BDS_ML.Models.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Respository
{
    interface IPostRepository: IDisposable
    {
        List<Post> GetPostByCond(int cond);  
        Post GetPostByID(int postId);  
        void InsertPost(Post post);  
        void UpdatePost(Post post);  
        int getCount(int id);  
        void Save();
    }
}
