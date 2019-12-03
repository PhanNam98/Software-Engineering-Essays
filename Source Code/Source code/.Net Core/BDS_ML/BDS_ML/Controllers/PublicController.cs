using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDS_ML.Controllers
{
    public class PublicController : Controller
    {
        private readonly BDT_MLDBContext _context;
        public PublicController() {
            _context = new BDT_MLDBContext();
        }
        [AllowAnonymous]
        public JsonResult Get_district(int province_id)
        {
            var list = _context.district.Where(p => p._province_id == province_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }
        [AllowAnonymous]
        public JsonResult Get_ward(int province_id, int district_id)
        {
            var list = _context.ward.Where(p => p._province_id == province_id && p._district_id == district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }
        [AllowAnonymous]
        public JsonResult Get_street(int province_id, int district_id)
        {
            var list = _context.street.Where(p => p._province_id == province_id && p._district_id == district_id);
            return Json(list.Select(x => new
            {
                ID = x.id,
                Name = x._prefix + " " + x._name
            }).ToList());
        }
    }
}