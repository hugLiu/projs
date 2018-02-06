using Jurassic.RECP.DataService;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    /// <summary>
    /// 用户个人中心
    /// </summary>
    public class RECPUserBehaviorController : BaseController
    {
        private IUserBehaviorService UserBehavior { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userBehavior"></param>
        public RECPUserBehaviorController(IUserBehaviorService userBehavior)
        {
            UserBehavior = userBehavior;
        }
        // GET: RECPUserBehavior
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户操作信息
        /// </summary>
        /// <param name="type">收藏"favorite" 下载"download"</param>
        /// <returns></returns>
        public JsonResult GetOptionMessByUserId(string type)
        {
            int userId = Convert.ToInt32(CurrentUser.Id);
            var list = UserBehavior.QueryOptionMessByUserId(userId, type);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据收藏列表的流水号Id拿到收藏记录
        /// </summary>
        /// <param name="RecNo">收藏列表的流水号</param>
        /// <returns></returns>
        public JsonResult GetOptionMessByRecNo(Guid RecNo)
        {
            var result = UserBehavior.GetOptionMessByRecNo(RecNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}