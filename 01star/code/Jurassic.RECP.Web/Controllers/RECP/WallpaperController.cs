using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class WallpaperController : Controller
    { 
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "wallpaper";
            return View();
        }
    }
}