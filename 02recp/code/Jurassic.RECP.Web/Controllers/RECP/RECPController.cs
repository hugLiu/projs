using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Jurassic.AppCenter;
using System.Collections.Generic;
using Jurassic.WebFrame;
using Jurassic.So.GeoTopic.Web.Controllers;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class RECPController : BaseController
    {
        private static readonly WebServiceController Services = new WebServiceController();
        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            if(User==null|| string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            return View();
        } 
        /// <summary>
        /// 转换资源文件路径
        /// </summary>
        /// <returns></returns>
        public JavaScriptResult TransResourceDefaultUrl()
        {
            var resourceDefaultUrl = ConfigurationManager.AppSettings["ZYResource"] != null ? ConfigurationManager.AppSettings["ZYResource"].ToString() : "";
            var data = new
            {
                ResourceDefaultUrl = resourceDefaultUrl
            };

            var js = "var defaultUrl = " + new JavaScriptSerializer().Serialize(data);
            return JavaScript(js);
        }

        public JsonResult GetMateData(string iiid)
        {
            var prams = new Dictionary<string, string> { { "iiid", iiid } };
            var metadata = Services.GetMateData(prams);
            return Json(metadata.Content,JsonRequestBehavior.AllowGet);
        }
    }
}
