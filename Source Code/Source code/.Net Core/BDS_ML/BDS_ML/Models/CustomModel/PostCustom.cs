using BDS_ML.Models.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.CustomModel
{
    public class PostCustom
    {
        public PostCustom()
        {

        }
        public PostCustom(Post Post, string StatusPost)
        {
            this.post = Post;
            this.statusPost = StatusPost;
        }
        public Post post { get; set; }
        public string statusPost { get; set; }
    }
}
