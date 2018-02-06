using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers
{
    public class CreationController : Controller
    { 
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "creation";
            return View();
        }
    }
}