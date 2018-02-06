using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataService.Tool; 

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class FavoriteCatalogService : IFavoriteCatalogService
    {
        /// <summary>
        /// 对收藏文件夹的所有操作
        /// </summary>
        private IGT_FavoriteCatalogEFRepository FavoriteCatalog { get; set; }
        private IUserBehaviorEFRepository UserBehaviorEFRepository { get; set; }
        private IUserBehaviorService UserBehaviorService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="favoriteCatalog"></param>
        public FavoriteCatalogService(IGT_FavoriteCatalogEFRepository favoriteCatalog, IUserBehaviorService userBehaviorService)
        {
            FavoriteCatalog = favoriteCatalog;
            UserBehaviorService = userBehaviorService;
        }
        /// <summary>
        /// 拿到收藏文件夹表的所有内容
        /// </summary>
        /// <returns></returns>
        public List<User_FavoriteCatalogModel> GetFavoriteCatalogMess()
        {
            var result = FavoriteCatalog.GetQuery().ToList();
            return result.Select(AutoMapper.Mapper.Map<RECP_User_FavoriteCatalog, User_FavoriteCatalogModel>).ToList();
        }
        public List<User_FavoriteCatalogModel> GetFavoriteCatalogMessByFavCatalogId(Guid FavCatalogId)
        {
            var result = FavoriteCatalog.GetQuery().Where(t => t.FavCatalogId == FavCatalogId).ToList();
            return result.Select(AutoMapper.Mapper.Map<RECP_User_FavoriteCatalog, User_FavoriteCatalogModel>).ToList();
        }
        /// <summary>
        /// 增加文件夹
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="json">要增加的文件夹的名字</param>
        public void AddFavoriteCatalog(int userId,string json)
        {
            AddFavoriteCatalogModel models = JsonUtil.JsonToObject(json, typeof(AddFavoriteCatalogModel)) as AddFavoriteCatalogModel; //页面上用户输入
            var result = new RECP_User_FavoriteCatalog
            {
                FavCatalogId= Guid.NewGuid(),
                UserId = userId,
                FavName = models.CatalogName,
                CreatedDate= DateTime.Now
            };
            FavoriteCatalog.Add(result);
        }
        /// <summary>
        /// 修改收藏文件夹
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="json">修改后文件夹的名字</param>
        /// <param name="CatalogId">修改收藏文件夹的Id</param>
        public void ModifyFavoriteCatalog(int userId,string json ,Guid CatalogId)
        {
            AddFavoriteCatalogModel models = JsonUtil.JsonToObject(json, typeof(AddFavoriteCatalogModel)) as AddFavoriteCatalogModel; //页面上用户输入
            var result = FavoriteCatalog.Find(t => t.FavCatalogId == CatalogId);
            if (result == null) return;
            result.FavName = models.CatalogName;
            FavoriteCatalog.Update(result);
        }
        /// <summary>
        /// 删除一个收藏文件夹
        /// </summary>
        /// <param name="userId">当前用户</param>
        /// <param name="Id">要删除的收藏文件夹的Id</param>
        public void RemoveFavoriteCatalog(int userId,Guid Id)
        {
            
            var result = FavoriteCatalog.Find(t => t.FavCatalogId == Id);
            if (result == null) return;
            else
            {
                FavoriteCatalog.Delete(result);
            }
        }
    }

    public class AddFavoriteCatalogModel
    {
        public string CatalogName { get; set; }
    }
}
