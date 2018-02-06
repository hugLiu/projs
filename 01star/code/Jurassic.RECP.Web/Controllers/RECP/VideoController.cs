using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class VideoController : Controller
    {
        /// <summary>
        /// 服务层接口
        /// </summary>
        private IVideoService videoService { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary> 
        public VideoController(IVideoService _videoService)
        {
            videoService = _videoService;
        }
        
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "video";
            return View();
        }

        /// <summary>
        /// 获取全部信息
        /// </summary>
        public JsonResult GetVideoInfo()
        { 
            var list = videoService.GetVideoInfo(null).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}