using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class AlbumController : Controller
    {
        /// <summary>
        /// 服务层接口
        /// </summary>
        private IAlbumService albumService { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        public AlbumController(IAlbumService _albumService)
        {
            albumService = _albumService;
        }

        public ActionResult Index()
        {
            ViewBag.ActiveTab = "album";
            return View();
        }
    }
}