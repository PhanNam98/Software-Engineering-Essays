using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models.ModelDB;

namespace BDS_ML.Models.Data
{
    public class Home_Index
    {
        public List<Post> lst3HotPosts { get; set; }
        public List<Post> lst6PopularPosts { get; set; }
        public Home_Index() { }

    }
}
