using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase.Service;
using System.Data.Entity;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataService.Enum;
using Jurassic.So.GeoTopic.Web.Controllers;
using Jurassic.So.GeoTopic.Web.Utility;
using Jurassic.So.Infrastructure.Util;
using Newtonsoft.Json;
using Jurassic.RECP.DataService.Tool;

namespace Jurassic.RECP.DataService
{
    public class UserBehaviorService : IUserBehaviorService
    {
        private IUserBehaviorEFRepository UserBehavior { get; set; }
        private IGT_UserBehaviorEFRepository gt_UserBehavior { get; set; }
        private IGT_FavoriteCatalogEFRepository FavoriteCatalog { get; set; }
        private static readonly WebServiceController services = new WebServiceController();
        public UserBehaviorService(IGT_UserBehaviorEFRepository GT_UserBehavior, IUserBehaviorEFRepository userBehavior, IGT_FavoriteCatalogEFRepository favoriteCatalog)
        {
            gt_UserBehavior = GT_UserBehavior;
            UserBehavior = userBehavior;
            FavoriteCatalog = favoriteCatalog;
        }
        /// <summary>
        /// 拿到下载或者收藏或者浏览的所有记录
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="type">可选为 收藏 下载 浏览</param>
        /// <returns></returns>
        public List<User_BehaviorModel> QueryOptionMessByUserId(int userId, string type)
        {
            List<RECP_User_Behavior> result;
            if (type == "favorite")
            {
                result = UserBehavior.GetQuery().Where(t => t.UserId == userId && t.FlagFavorite == 1).ToList();
            }
            else if (type == "download")
            {
                result = UserBehavior.GetQuery().Where(t => t.UserId == userId && t.FlagDownload == 1).ToList();
            }
            else if (type == "look")
            {
                result = UserBehavior.GetQuery().Where(t => t.UserId == userId && t.FlagLook == 1).ToList();
            }
            else
            {
                return null;
            }
            return result.Select(AutoMapper.Mapper.Map<RECP_User_Behavior, User_BehaviorModel>).ToList();
        }
        /// <summary>
        /// 根据收藏文件夹的目录拿到该目录下所有的收藏记录
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="catalogId">收藏文件夹Id</param>
        /// <param name="type">类型可选为 收藏 下载 浏览</param>
        /// <returns></returns>
        public List<User_BehaviorModel> GetOptionMessByCatalogId(int userId, Guid catalogId, string type)
        {
            List<RECP_User_Behavior> result;
            if (type == "favorite")
            {
                result = UserBehavior.GetQuery().Where(t => t.FavCatalogId == catalogId && t.FlagFavorite == 1 && t.UserId == userId).ToList();
            }
            else if (type == "download")
            {
                result = UserBehavior.GetQuery().Where(t => t.FavCatalogId == catalogId && t.FlagDownload == 1 && t.UserId == userId).ToList();
            }
            else if (type == "look")
            {
                result = UserBehavior.GetQuery().Where(t => t.FavCatalogId == catalogId && t.FlagLook == 1 && t.UserId == userId).ToList();
            }
            else
            {
                return null;
            }
            return result.Select(AutoMapper.Mapper.Map<RECP_User_Behavior, User_BehaviorModel>).ToList();
        }
        /// <summary>
        /// 将浏览的记录或者是下载的记录放到收藏文件夹中
        /// </summary>
        /// <param name="RecNo">收藏列表的流水号</param>
        /// <param name="FavCatalogId">收藏文件夹的流水号</param>
        public Collection collect(Guid RecNo, Guid FavCatalogId)
        {
            var result = UserBehavior.Find(t => t.RecNo == RecNo);
            if (result == null) return Collection.CollectCancel;
            //之前收藏过
            if (result.FlagFavorite == 1)
            {
                result.FavCatalogId = FavCatalogId;
                UserBehavior.Update(result);
                return Collection.CollectSuccess;
            }
            //之前未收藏过
            else
            {
                result.FlagFavorite = 1;
                result.FavCatalogId = FavCatalogId;
                UserBehavior.Update(result);
                return Collection.CollectSuccess;
            }

        }
        /// <summary>
        /// 根据收藏列表的流水号拿到收藏记录
        /// </summary>
        /// <param name="RecNo">收藏列表的流水号</param>
        /// <returns></returns>
        public List<User_BehaviorModel> GetOptionMessByRecNo(Guid RecNo)
        {
            var result = UserBehavior.GetQuery().Where(t => t.RecNo == RecNo);
            return result.Select(AutoMapper.Mapper.Map<RECP_User_Behavior, User_BehaviorModel>).ToList();
        }

        public List<User_BehaviorModel> GetUserBehaviorGttop(int count, string type)
        {
            if (type == "look")
            {
                var result = UserBehavior.GetQuery().Where(t => t.FlagLook == 1).Take(count).OrderByDescending(t => t.CreatedDate);
                return result.Select(AutoMapper.Mapper.Map<RECP_User_Behavior, User_BehaviorModel>).ToList();
            }
            else if (type == "download")
            {
                var list = new List<User_BehaviorModel>();
                var gt = gt_UserBehavior.GetQuery().Where(t => t.BehaviorType == type).GroupBy(x => new { x.UserId, x.Content })
                    .Select(x => new { x.Key, cnt = x.Count() }).OrderByDescending(x => x.cnt).Take(count).ToList();
                #region gt content 字段转换
                var gtList = new List<User_BehaviorModel>();
                foreach (var g in gt)
                {
                    var content = g.Key.Content;
                    var contentModel = JsonUtil.JsonToObject(content, typeof(GT_UserBehaviorContentModel)) as GT_UserBehaviorContentModel;
                    if (contentModel != null)
                    {
                        var gtModel = new User_BehaviorModel
                        {
                            iiid = contentModel.id,
                            UserId = g.Key.UserId,
                            DC_Title_Text = contentModel.title,
                            DC_Author = "",
                            CountDownload = g.cnt
                        };
                        gtList.Add(gtModel);
                    }
                }
                #endregion
                var recpList = UserBehavior.GetQuery().Where(t => t.FlagDownload == 1).OrderByDescending(t => t.CountDownload).Take(count)
                    .Select(x => new User_BehaviorModel { iiid = x.iiid, UserId = x.UserId, DC_Title_Text = x.DC_Title_Text, DC_Author = x.DC_Author, CountDownload = x.CountDownload }).ToList();
                var totalList = gtList.Concat(recpList).OrderByDescending(x => x.CountDownload).ToList();
                foreach (var tl in totalList)
                {
                    if (list.Exists(x => x.iiid == tl.iiid && x.UserId == tl.UserId))
                    {
                        var index = list.FindIndex(x => x.iiid == tl.iiid && x.UserId == tl.UserId);
                        list[index].CountDownload += tl.CountDownload;
                    }
                    else
                    {
                        list.Add(tl);
                    }
                }
                list = list.OrderByDescending(x => x.CountDownload).Take(count).ToList();
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 成果列表的收藏
        /// </summary>
        /// <param name="model"></param>
        public Collection collectList(AchieveListModel model, int userId, string _apiPath, string _searchService, string _getMetaData)
        {
            var favCatalogData = FavoriteCatalog.GetQuery().ToList();
            //如果没有收藏文件夹
            if (favCatalogData.Count == 0)
            {
                var favCatalog = new RECP_User_FavoriteCatalog
                {
                    FavCatalogId = Guid.NewGuid(),
                    UserId = userId,
                    FavName = "新建收藏夹",
                    CreatedDate = DateTime.Now
                };
                FavoriteCatalog.Add(favCatalog);
                favCatalogData = FavoriteCatalog.GetQuery().ToList();
            }
            //如果存在收藏文件夹 默认选择第一条
            else
            {
                favCatalogData = favCatalogData.Take(1).ToList();
            }
            var userBehaviorData = UserBehavior.Find(t => t.iiid == model.iiid);
            if (userBehaviorData == null)
            {
                //通过iiid取数据
                var prams = new Dictionary<string, string> { { "iiid", model.iiid } };
                //string userToken = TokenManmge.GetTokenService();
                //var result = WebRequestUtil.GetHttpClientStr(_apiPath + _searchService + _getMetaData, prams, userToken).Result;
                //RECP_User_Behavior data= JsonConvert.DeserializeObject<RECP_User_Behavior>(result);

                //var metadata = services.GetMateData(prams);
                //var kmd = new KMD(metadata.Content);
                //之前没有收藏过就收藏
                var userBehavior = new RECP_User_Behavior
                {
                    RecNo = Guid.NewGuid(),
                    UserId = userId,
                    iiid = model.iiid,
                    EP_ProductType = "",
                    DC_Title_Text = "",
                    DC_Author = "",
                    DC_Date_Created = DateTime.Now,
                    IndexedDate = DateTime.Now,
                    FlagLook = 0,
                    CountLook = 0,
                    FlagFavorite = 1,
                    FavCatalogId = favCatalogData[0].FavCatalogId,
                    FlagDownload = 0,
                    CountDownload = 0,
                    CreatedDate = DateTime.Now,
                    State = 1
                };
                UserBehavior.Add(userBehavior);
                return Collection.CollectSuccess;
            }
            else  //之前收藏过，就取消收藏
            {
                UserBehavior.Delete(userBehaviorData);
                return Collection.CollectCancel;
            }
        }

        /// <summary>
        /// 成果导航成果列表
        /// 功能一：拿到某一个iiid被所有用户浏览的总数
        /// 功能二：检测当前用户是否收藏过该成果
        /// </summary>
        /// <param name="iiid">成果的iiid</param>
        /// <param name="userId">当前用户ID</param>
        public AchieveListOtherItemModel GetSkimCountByiiid(string iiid, int userId)
        {
            var result = new AchieveListOtherItemModel();
            var countLook = 0;  //浏览次数-->不指定某个特定的用户
            var flagFav = 0;
            var userBehavDataByiiid = UserBehavior.Find(t => t.iiid == iiid);
            if (userBehavDataByiiid == null) //如果找不到这条成果
            {
                countLook = 0;
            }
            else  //如果找到这条成果
            {
                countLook = UserBehavior.GetQuery().Where(t => t.iiid == iiid).Sum(t => t.CountLook);
            }
            var flagFavorite = UserBehavior.GetQuery().Where(t => t.iiid == iiid && t.UserId == userId && t.FlagFavorite == 1).ToList();
            if (flagFavorite.Count == 0)  //当前用户没有收藏这条成果
            {
                flagFav = 0;
            }
            else  //当前用户收藏了这条成果
            {
                flagFav = 1;
            }
            result.countLook = countLook;
            result.flagFav = flagFav;
            return result;
        }
    }
}
