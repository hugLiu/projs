using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface IFavoriteCatalogService
    {
        /// <summary>
        /// 拿到收藏文件夹表的所有内容
        /// </summary>
        /// <returns></returns>
        List<User_FavoriteCatalogModel> GetFavoriteCatalogMess();
        /// <summary>
        /// 根据收藏文件夹Id拿到那一条收藏文件夹的所有信息
        /// </summary>
        /// <param name="FavCatalogId"></param>
        /// <returns></returns>
        List<User_FavoriteCatalogModel> GetFavoriteCatalogMessByFavCatalogId(Guid FavCatalogId);
        /// <summary>
        /// 删除一个收藏文件夹
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="Id">要删除的收藏文件夹的Id</param>
        void RemoveFavoriteCatalog(int userId,Guid Id);
        /// <summary>
        /// 增加一个收藏文件夹
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="json">要增加的收藏文件夹的名字</param>
        void AddFavoriteCatalog(int userId,string json);
        /// <summary>
        /// 修改收藏文件夹
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="json">要修改的收藏文件夹的名字</param>
        /// <param name="CatalogId">要修改的收藏文件夹的Id</param>
        void ModifyFavoriteCatalog(int userId,string json,Guid CatalogId);
    }
}
