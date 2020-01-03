using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.Builder
{
    public interface IBuilder
    {
        void BuildDetailPost(int ID_Post, bool alley, int bedroom, int yard, int floor, bool nearSchool, bool nearAirport, bool nearHospital, bool nearMarket,
            string descriptiondetail, int bathroom);

        void BuildLocationPost(int ID_Post, int district, int ward, int street, string diachi, int province, int? project);


        void BuildImagePost(int ID_Post, string ID_User, List<IFormFile> images);


        void BuildStatusPost(string ID_User, int ID_Post, int Role);
       
    }
}
