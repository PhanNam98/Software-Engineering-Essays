using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDS_ML.Areas.Admin.Controllers
{
    
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}