using Jurassic.AppCenter;
using Jurassic.RECP.DataService;
using Jurassic.RECP.DataService.Service;
using Jurassic.RECP.DataService.Tool;
using System;
using System.Web.Mvc;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Service;
using Jurassic.WebFrame;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    /// 个人中心
    public class UserController : BaseController
    {
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"]
            + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];
        private readonly string _achieveDown = System.Configuration.ConfigurationManager.AppSettings["AchieveDown"];
        private IUserBehaviorService UserBehavior { get; set; }
        private Iwebpages_MembershipService Webpages_Membership { get; set; }
        private IFavoriteCatalogService FavoriteCatalog { get; set; }
        private IUserBehaviorEFRepository UserBehaviorEF { get; set; }
        private IUserProfileService userProfileService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userBehavior">用户行为表</param>
        /// <param name="webpages_Membership">用于存储用户密码表</param>
        /// <param name="favoriteCatalog">收藏文件夹表</param>
        public UserController(IUserBehaviorService userBehavior, Iwebpages_MembershipService webpages_Membership, IFavoriteCatalogService favoriteCatalog, IUserBehaviorEFRepository userBehaviorEF, IUserProfileService _userProfileService)
        {
            UserBehavior = userBehavior;
            Webpages_Membership = webpages_Membership;
            FavoriteCatalog = favoriteCatalog;
            UserBehaviorEF = userBehaviorEF;
            userProfileService = _userProfileService;
        }
        /// 主页面
        public ActionResult Index()
        {
            var _userName = User.Identity.GetUserName();
            ViewBag.username = _userName;
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }
        /// 获取用户操作信息
        public JsonResult GetOptionMessByUserId(string type)
        {
            int userId = Convert.ToInt32(CurrentUser.Id);
            var list = UserBehavior.QueryOptionMessByUserId(userId, type);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// 根据收藏列表的流水号Id拿到收藏记录
        public JsonResult GetOptionMessByRecNo(Guid RecNo)
        {
            var result = UserBehavior.GetOptionMessByRecNo(RecNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// 修改密码验证
        public String checkPass(string json)
        {
            var state = ResetPassword(json);
            if (state == ChangePasswordState.OK)
                return "Success";
            else
                return "Failure";
        }
        /// 修改密码
        public ChangePasswordState ResetPassword(string json)
        {
            OldPassModel models = JsonUtil.JsonToObject(json, typeof(OldPassModel)) as OldPassModel; //页面上用户输入
            var _userId = int.Parse(User.Identity.GetUserId());
            var passMess = Webpages_Membership.GetPassMessByUserId(_userId);  //根据当前用户Id拿到数据库中密码信息
            var password = models.pwdOld;     //用户输入的密码
            var passwordSalt = passMess[0].PasswordSalt;  //数据库中密码盐值
            var passwordEncrypted = Encryption.MD5(password + passwordSalt);  //用户输入密码+密码盐值 MD5加密

            if (passwordEncrypted != passMess[0].Password)
            {
                return ChangePasswordState.OldPasswordError;
            }

            var newPass = Encryption.MD5(models.pwdNew + passwordSalt);
            Webpages_Membership.ModifyUserPass(_userId, newPass);
            return ChangePasswordState.OK;
        }

        public String GetPassword(string oldPwd)
        {
            var _userId = int.Parse(User.Identity.GetUserId());
            var passMess = Webpages_Membership.GetPassMessByUserId(_userId);
            if (Encryption.MD5(oldPwd + passMess[0].PasswordSalt) != passMess[0].Password)
            {
                return "fail";
            }
            else
            {
                return "success";
            }
        }
        /// 获取所有的收藏目录
        public JsonResult GetFavoriteCatalogMess()
        {
            var FavoriteCatalogMess = FavoriteCatalog.GetFavoriteCatalogMess();
            return Json(FavoriteCatalogMess, JsonRequestBehavior.AllowGet);
        }
        /// 根据收藏目录流水号拿到那一条收藏目录的信息
        public JsonResult GetFavoriteCatalogMessByFavCatalogId(Guid FavCatalogId)
        {
            var result = FavoriteCatalog.GetFavoriteCatalogMessByFavCatalogId(FavCatalogId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// 通过收藏目录的Id拿到收藏文件夹下的所有信息
        public JsonResult GetFavoriteMessByCatalogId(Guid id, string type)
        {
            var userId = int.Parse(User.Identity.GetUserId()); //当前用户Id
            var FavoriteMessByCatalogId = UserBehavior.GetOptionMessByCatalogId(userId, id, type);
            return Json(FavoriteMessByCatalogId, JsonRequestBehavior.AllowGet);
        }
        /// 增加收藏文件夹
        public void AddFavoriteCatalog(string json)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            FavoriteCatalog.AddFavoriteCatalog(userId, json);
        }
        ///  修改收藏文件夹
        public void ModifyFavoriteCatalog(string json, Guid CatalogId)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            FavoriteCatalog.ModifyFavoriteCatalog(userId, json, CatalogId);
        }
        /// 删除收藏文件夹
        public void RemoveFavoriteCatalog(Guid Id)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            FavoriteCatalog.RemoveFavoriteCatalog(userId, Id);
        }
        /// 将浏览的记录或者下载的记录放到收藏文件夹中
        public Collection Collect(Guid RecNo, Guid FavCatalogId)
        {
            var result = UserBehavior.collect(RecNo, FavCatalogId);
            return result;
        }
        /// 这个方法主要是拿到组件需要的数据  最近浏览组件 下载历史组件
        public JsonResult GetUserBehaviorGttop(int count, string type)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            var result = UserBehavior.GetUserBehaviorGttop(count, type);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// 打开详情展示页面
        public OpenDetail OpenDetailPage(string iiid, string name, string author, string producttype)
        {
            ////在这里只给用户提供收藏和取消收藏的功能
            var userId = int.Parse(User.Identity.GetUserId());
            //判断当前用户有没有这条成果
            var userBehaviorData = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId);
            if (userBehaviorData == null)
            {
                var userBehavior = new RECP_User_Behavior
                {
                    RecNo = Guid.NewGuid(),
                    UserId = userId,
                    iiid = iiid,
                    EP_ProductType = producttype == "" ? DBNull.Value.ToString() : producttype,
                    DC_Title_Text = name == "" ? DBNull.Value.ToString() : name,
                    DC_Author = author == "" ? DBNull.Value.ToString() : author,
                    DC_Date_Created = DateTime.Now,
                    IndexedDate = DateTime.Now,
                    FlagLook = 1,
                    CountLook = 1,
                    FlagFavorite = 0,
                    FavCatalogId = null,
                    FlagDownload = 0,
                    CountDownload = 0,
                    CreatedDate = DateTime.Now,
                    State = 1
                };
                UserBehaviorEF.Add(userBehavior);
                return OpenDetail.Success;
            }
            else
            {
                var data = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId && t.FlagLook == 1);
                //有这条成果但是没有浏览过
                if (data == null)
                {
                    userBehaviorData.FlagLook = 1;
                    userBehaviorData.CountLook = 1;
                    UserBehaviorEF.Update(userBehaviorData);
                    return OpenDetail.Success;
                }
                else
                {
                    userBehaviorData.CountLook += 1;
                    UserBehaviorEF.Update(data);
                    return OpenDetail.Success;
                }
            }
        }

        /// <summary>
        /// 只适用项目本身的列表信息的下载情况
        /// </summary>
        /// <param name="iiid"></param>
        /// <param name="name"></param>
        /// <param name="author"></param>
        /// <param name="producttype"></param>
        public void DownLoadRecords(string iiid, string name, string author, string producttype)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            //判断当前用户有没有这条成果
            var userBehaviorData = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId);
            if (userBehaviorData == null)
            {
                var userBehavior = new RECP_User_Behavior
                {
                    RecNo = Guid.NewGuid(),
                    UserId = userId,
                    iiid = iiid,
                    EP_ProductType = producttype == "" ? DBNull.Value.ToString() : producttype,
                    DC_Title_Text = name == "" ? DBNull.Value.ToString() : name,
                    DC_Author = author == "" ? DBNull.Value.ToString() : author,
                    DC_Date_Created = DateTime.Now,
                    IndexedDate = DateTime.Now,
                    FlagLook = 0,
                    CountLook = 0,
                    FlagFavorite = 0,
                    FavCatalogId = null,
                    FlagDownload = 1,
                    CountDownload = 1,
                    CreatedDate = DateTime.Now,
                    State = 1
                };
                UserBehaviorEF.Add(userBehavior);
            }
            else
            {
                var data = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId && t.FlagDownload == 1);
                //有这条成果但是没有下载过
                if (data == null)
                {
                    userBehaviorData.FlagDownload = 1;
                    userBehaviorData.CountDownload = 1;
                    UserBehaviorEF.Update(userBehaviorData);
                }
                else
                {
                    userBehaviorData.CountDownload += 1;
                    UserBehaviorEF.Update(data);
                }
            }
        } 

        /// <summary>
        /// 把gt用户记录存放到recp中
        /// </summary>
        /// <param name="iiid"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="createDate"></param>
        /// <param name="productType"></param>
        public void SaveGtUserBehavoir(string iiid, int userId, string title, string author, string createDate, string productType)
        {
            //判断当前用户有没有这条成果
            var userBehaviorData = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId);
            if (userBehaviorData == null)
            { 
                var userBehavior = new RECP_User_Behavior
                {
                    RecNo = Guid.NewGuid(),
                    UserId = userId,
                    iiid = iiid,
                    EP_ProductType = productType,
                    DC_Title_Text = title,
                    DC_Author = author,
                    DC_Date_Created = DateTime.Now,
                    IndexedDate = DateTime.Now,
                    FlagLook = 0,
                    CountLook = 0,
                    FlagFavorite = 0,
                    FavCatalogId = null,
                    FlagDownload = 1,
                    CountDownload = 1,
                    CreatedDate = DateTime.Now,
                    State = 1
                };
                UserBehaviorEF.Add(userBehavior);
            }
            else
            {
                var data = UserBehaviorEF.Find(t => t.iiid == iiid && t.UserId == userId && t.FlagDownload == 1);
                //有这条成果但是没有下载过
                if (data == null)
                {
                    userBehaviorData.FlagDownload = 1;
                    userBehaviorData.CountDownload = 1;
                    UserBehaviorEF.Update(userBehaviorData);
                }
                else
                {
                    data.CountDownload += 1;
                    UserBehaviorEF.Update(data);
                }
            }
        }
        /// <summary>
        /// 【系统管理】 菜单的权限控制
        /// </summary>
        /// <returns></returns>
        public string GetRoleName()
        {
            var id = int.Parse(User.Identity.GetUserId());
            var name = userProfileService.GetRoleName(new UserProfileModel { UserId = id });
            return name;
        }
    }

    public class OldPassModel
    {
        public string pwdOld { get; set; }
        public string pwdNew { get; set; }
        public string pwdConfirm { get; set; }
    }

    public enum OpenDetail
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Fail
    }
}