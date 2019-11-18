using BDS_ML.Models.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.CustomModel
{
    public class PostFullDetail
    {
        public PostFullDetail() { }
        private Post posT;
        private Post_Status postStatus;
        private string status;
        private Post_Location postLocation;
        private Post_Detail postDetail;
        private List<Post_Image> listPostImage;

        public Post_Status PostStatus { get => postStatus; set => postStatus = value; }
        public Post PosT { get => posT; set => posT = value; }
        public Post_Location PostLocation { get => postLocation; set => postLocation = value; }
        public Post_Detail PostDetail { get => postDetail; set => postDetail = value; }
        public List<Post_Image> ListPostImage { get => listPostImage; set => listPostImage = value; }
    }
}