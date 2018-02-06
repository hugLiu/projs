using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Jurassic.AppCenter;
using System.Collections.Generic;
using Jurassic.WebFrame; 

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class RECPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        } 
    }
}
