using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomePageController : Controller
    {
        /// <summary>
        /// 视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}