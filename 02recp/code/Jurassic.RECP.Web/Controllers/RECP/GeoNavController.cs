using System;
using System.Linq;
using System.Web.Mvc;
using Jurassic.RECP.DataService;
using Jurassic.WebFrame;
using Jurassic.AppCenter;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataBase.Service;
using Jurassic.So.GeoTopic.Web.Controllers;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataService.Service;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class GeoNavController : BaseController
    {
        // GET: /WebService/
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"]
            + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];
        private readonly string _achieveDown = System.Configuration.ConfigurationManager.AppSettings["AchieveDown"];
        private readonly string _searchService = System.Configuration.ConfigurationManager.AppSettings["SearchService"];
        private readonly string _getMetaData = System.Configuration.ConfigurationManager.AppSettings["GetMetaData"];

        private static readonly WebServiceController services = new WebServiceController();
        private IUserBehaviorEFRepository UserBehaviorEFRepository { get; set; }
        private IGT_FavoriteCatalogEFRepository FavoriteCatalogEFRepository { get; set; }
        private IUserBehaviorService UserBehaviorService { get; set; }
        private IGT_TopicCardService GT_TopicCardService { get; set; }
        /// <summary>
        /// 成果导航控制器
        /// </summary>
        /// <param name="_UserBehaviorService">用户行为Service层</param>
        /// <param name="_UserBehaviorEFRepository">用户行为数据层</param>
        /// <param name="_FavoriteCatalogEFRepository"></param>
        /// <param name="IGT_TopicCardService"></param>
        public GeoNavController(IUserBehaviorService _UserBehaviorService, IUserBehaviorEFRepository _UserBehaviorEFRepository, IGT_FavoriteCatalogEFRepository _FavoriteCatalogEFRepository, IGT_TopicCardService IGT_TopicCardService)
        {
            UserBehaviorService = _UserBehaviorService;
            UserBehaviorEFRepository = _UserBehaviorEFRepository;
            FavoriteCatalogEFRepository = _FavoriteCatalogEFRepository;
            GT_TopicCardService = IGT_TopicCardService;
        }
        /// <summary>
        /// 成果导航视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }
        /// <summary>
        /// 规范标准视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Standard()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }

        /// <summary>
        /// 科研成果模块
        /// </summary>
        /// <returns></returns>
        public ActionResult Research()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }
        
        /// <summary>
        /// 成果列表收藏
        /// </summary>
        /// <param name="iiid"></param>
        /// <param name="name"></param>
        /// <param name="author"></param>
        /// <param name="producttype"></param>
        /// <param name="indexDate"></param>
        /// <param name="createdDate"></param>
        /// <returns></returns>
        public Collection Collect(string iiid,string name,string author,string producttype, DateTime indexDate,DateTime createdDate)
        {
            ////在这里只给用户提供收藏和取消收藏的功能
            var userId = int.Parse(User.Identity.GetUserId());
            
            //判断当前用户有没有这条成果
            var userBehaviorData = UserBehaviorEFRepository.Find(t => t.iiid == iiid && t.UserId== userId);
            if (userBehaviorData == null)
            {
                var userBehavior = new RECP_User_Behavior
                {
                    RecNo = Guid.NewGuid(),
                    UserId = userId,
                    iiid = iiid,
                    EP_ProductType = producttype,
                    DC_Title_Text = name,
                    DC_Author = author,
                    DC_Date_Created = createdDate,
                    IndexedDate = indexDate,
                    FlagLook = 0,
                    CountLook = 0,
                    FlagFavorite = 1,
                    FavCatalogId = null,
                    FlagDownload = 0,
                    CountDownload = 0,
                    CreatedDate = DateTime.Now,
                    State = 1
                };
                UserBehaviorEFRepository.Add(userBehavior);
                return Collection.CollectSuccess;
            }
            else  
            {
                var data = UserBehaviorEFRepository.Find(t => t.iiid == iiid && t.UserId == userId && t.FlagFavorite==1);
                //有这条成果但是这条成果没有收藏过
                if (data == null)
                {
                    userBehaviorData.FlagFavorite = 1;
                    UserBehaviorEFRepository.Update(userBehaviorData);
                    return Collection.CollectSuccess;
                }else
                {
                    data.FlagFavorite = 0;
                    data.FavCatalogId = null;
                    UserBehaviorEFRepository.Update(data);
                    return Collection.CollectCancel;
                }
            }
        }
        /// <summary>
        /// 拿到某一个成果的总浏览次数，包括所有用户的
        /// </summary>
        /// <param name="iiid"></param>
        /// <returns></returns>
        public JsonResult GetSkimCountByiiid(string iiid)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            var result= UserBehaviorService.GetSkimCountByiiid(iiid,userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过树节点的ID拿到成果类型
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public JsonResult GetProductTypeByTopicId(int topicId)
        {
            var result = GT_TopicCardService.GetProductTypeByTopicId(topicId);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
    }
}