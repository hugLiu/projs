using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;

namespace Jurassic.RECP.DataService
{
    public interface IUserBehaviorService
    {
        /// <summary>
        /// 拿到下载或者收藏或者浏览的所有记录
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="type">收藏"favorite" 下载"download"</param>
        /// <returns></returns>
        List<User_BehaviorModel> QueryOptionMessByUserId(int userId, string type);
        /// <summary>
        /// 根据收藏文件夹的目录拿到该目录下所有的收藏记录
        /// </summary>
        /// <param name="catalogId">收藏文件夹Id</param>
        /// <param name="type">类型 favorite download</param>
        /// <returns></returns>
        List<User_BehaviorModel> GetOptionMessByCatalogId(int userId, Guid catalogId, string type);
        /// <summary>
        /// 将浏览的记录或者是下载的记录放到收藏文件夹中
        /// </summary>
        /// <param name="RecNo">收藏列表的流水号</param>
        /// <param name="FavCatalogId">收藏文件件的流水号</param>
        Collection collect(Guid RecNo, Guid FavCatalogId);
        /// <summary>
        /// 根据收藏列表的流水号拿到那列收藏记录
        /// </summary>
        /// <param name="RecNo">收藏列表的流水号</param>
        /// <returns></returns>
        List<User_BehaviorModel> GetOptionMessByRecNo(Guid RecNo);
        List<User_BehaviorModel> GetUserBehaviorGttop(int count, string type);
        /// <summary>
        /// 收藏成果列表
        /// </summary>
        /// <param name="model"></param>
        Collection collectList(AchieveListModel model,int userId, string _apiPath, string _searchService, string _getMetaData);
        /// <summary>
        /// 成果导航   通过某条成果的iiid拿到所有用户的总共浏览次数以及该成果是否被当前用户收藏过
        /// </summary>
        /// <param name="iiid">成果的iiid</param>
        /// <param name="userId">当前用户的ID</param>
        AchieveListOtherItemModel GetSkimCountByiiid(string iiid,int userId);
    }
}
