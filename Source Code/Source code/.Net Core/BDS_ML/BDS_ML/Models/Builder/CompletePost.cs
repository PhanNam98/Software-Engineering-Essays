using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.Builder
{
    public class CompletePost
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }


        public void buildFullPost(string ID_User,int ID_Post
            , int district, int ward, int street, string diachi, bool alley, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket, List<IFormFile> images, string descriptiondetail, int bathroom,
            int bedroom, int yard, int floor, int province, int Role, int? project)
        {
            this._builder.BuildDetailPost(ID_Post, alley, bedroom, yard, floor, nearSchool, nearAirport, nearHospital, nearMarket,
            descriptiondetail,bathroom);
            this._builder.BuildLocationPost(ID_Post, district, ward, street, diachi, province, project);
            this._builder.BuildImagePost(ID_Post, ID_User, images);
            this._builder.BuildStatusPost(ID_User, ID_Post,Role);
        }
    }
}
