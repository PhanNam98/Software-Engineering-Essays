using BDS_ML.Data;
using BDS_ML.Models.ModelDB;
using BDS_ML.Respository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models
{
    public class PostIndex
    {
        private IPostRepository _postRepository;

        public PostIndex()
        {
            this._postRepository = new PostRespository();
        }

        public PostIndex(BDT_MLDBContext db)
        {
        }

        public List<Post> get3HotPosts()
        {
            return _postRepository.GetPostByCond(3);
        }

        public List<Post> get6PopularPosts()
        {
            return _postRepository.GetPostByCond(6);
        }

        public int getCount(int id)
        {
            return _postRepository.getCount(id);
        }
    }
}
