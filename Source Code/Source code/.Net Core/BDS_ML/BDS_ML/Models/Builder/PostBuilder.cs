using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.Builder
{
    public class PostBuilder : IBuilder
    {
        private RealStatePost _post = new RealStatePost();
        public void BuildDetailPost(int ID_Post, bool alley, int bedroom, int yard, int floor, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket,
            string descriptiondetail, int bathroom)
        {
            _post.Add_Post_Detail(ID_Post, alley, bedroom, yard, floor, nearSchool, nearAirport, nearHospital, nearMarket,
            descriptiondetail, bathroom);
        }

        public void BuildLocationPost(int ID_Post, int district, int ward, int street, string diachi, int province, int? project)
        {
            _post.Add_Post_Location(ID_Post, district, ward, street, diachi, province, project);
        }

        public void BuildImagePost(int ID_Post, string ID_User, List<IFormFile> images)
        {
            _post.Add_Post_Image(ID_Post, ID_User, images);
        }

        public void BuildStatusPost(string ID_User, int ID_Post, int Role)
        {
            _post.Add_Post_Status(ID_User, ID_Post, Role);
        }
    }
}
