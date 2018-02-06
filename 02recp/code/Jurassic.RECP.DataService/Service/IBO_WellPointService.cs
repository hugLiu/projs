using System.Collections.Generic;
using Jurassic.RECP.DataService.Models.View;

namespace Jurassic.RECP.DataService.Service
{
    public interface IBO_WellPointService
    {
        /// <summary>
        /// 获取井点分布视图数据接口
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO_WellPointModel> GetBaseInfoViewData(string bot, string name);
        /// 拿到BO_WellPoint这个视图下的所有井点数据
        /// </summary>
        /// <returns></returns>
        List<BO_WellPointModel> GetWellPointGridData();
        /// <summary>
        /// 通过树节点的SID拿到符合条件的井点数据
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        List<BO_WellPointModel> GetWellPointGridDataBySID(string sid);
        /// <summary>
        /// 通过搜索条件拿到井点的搜索结果
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pid"></param>
        /// <param name="bolckGuan">井区</param>
        /// <param name="blockType">井别</param>
        /// <param name="blockName">井名的模糊匹配</param>
        /// <returns></returns>
        List<BO_WellPointModel> SearchWellPointData(string sid, string bolckGuan, string blockType, string blockName);
        /// <summary>
        /// 拿到井点的所有数据
        /// </summary>
        /// <returns></returns>
        List<BO_WellPointModel> GetAllData();
    }
}
