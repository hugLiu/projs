using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class AboutUsController : Controller
    { 
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "aboutus";
            return View();
        }
    }
}