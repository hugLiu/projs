using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Jurassic.RECP.DataBase.Models.View;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models.View;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class BO_WellPointService : IBO_WellPointService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IBO_WellPointEFRepository BO_WellPoint { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_BO_WellPoint"></param>
        public BO_WellPointService(IBO_WellPointEFRepository _BO_WellPoint)
        {
            BO_WellPoint = _BO_WellPoint;
        }

        public IEnumerable<BO_WellPointModel> GetBaseInfoViewData(string bot, string name)
        {
            //var BaseInfo = BO_WellPoint.GetQuery().ToList();
            //IEnumerable<v_BO_WellPoint> BaseInfo = BO_WellPoint.GetQuery();
            //if (string.IsNullOrWhiteSpace(bot))
            //{
            //    return null;
            //}
            //else
            //{
            //    if (bot.Trim() == "系组层")
            //    {
            //        //当为根节点的时候需要拿到什么
            //    }
            //    else if (bot.Trim() == "系")
            //    {
            //        BaseInfo = BaseInfo.Where(x => x.Series == name);
            //    }
            //    else if (bot.Trim() == "组")
            //    {
            //        BaseInfo = BaseInfo.Where(x => x.Formation == name);
            //    }
            //    else if (bot.Trim() == "层")
            //    {
            //        BaseInfo = BaseInfo.Where(x => x.Layer == name);
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}

            //return BaseInfo.Select(Mapper.Map<v_BO_WellPoint, BO_WellPointModel>).ToList();
            return null;
        }

        /// <summary>
        /// 拿到BO_WellPoint这个视图下的所有井点数据
        /// </summary>
        /// <returns></returns>
        public List<BO_WellPointModel> GetWellPointGridData()
        {
            var result = BO_WellPoint.GetQuery().ToList();
            return result.Select(Mapper.Map<v_BO_WellPoint, BO_WellPointModel>).ToList();
        }
        /// <summary>
        /// 通过树节点的SID信息匹配BO_WellPoint的parents
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<BO_WellPointModel> GetWellPointGridDataBySID(string sid)
        {
            var result = BO_WellPoint.GetQuery().Where(x => x.Parents.Contains(sid)).ToList();
            return result.Select(Mapper.Map<v_BO_WellPoint, BO_WellPointModel>).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pid"></param>
        /// <param name="bolckGuan">井区</param>
        /// <param name="blockType">井别</param>
        /// <param name="blockName">井名的模糊匹配</param>
        /// <returns></returns>
        public List<BO_WellPointModel> SearchWellPointData(string sid, string blockGuan, string blockType, string blockName)
        {
            List<v_BO_WellPoint> result = BO_WellPoint.GetQuery().ToList(); 
            if (!string.IsNullOrWhiteSpace(sid.ToString()))
            {
                result = result.Where(x => x.Parents.Contains(sid)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(blockGuan.ToString()) && blockGuan != "选择井区...")
            {
                result = result.Where(x => x.BlockGuan == blockGuan).ToList();
            }
            if (!string.IsNullOrWhiteSpace(blockType.ToString()) && blockType != "选择井别...")
            {
                result = result.Where(x => x.BlockType == blockType).ToList();
            }
            if (!string.IsNullOrWhiteSpace(blockName.ToString()))
            {
                result = result.Where(x => x.Name.ToLower().Contains(blockName.ToLower())).ToList();
            }
            return result.OrderBy(x => x.Name).Select(Mapper.Map<v_BO_WellPoint, BO_WellPointModel>).ToList();
        }

        public List<BO_WellPointModel> GetAllData()
        {
            var result = BO_WellPoint.GetQuery();
            return result.Select(Mapper.Map<v_BO_WellPoint, BO_WellPointModel>).ToList();
        }
    }
}
