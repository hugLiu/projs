using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface IBO_ProMetaDefineService
    {
        /// <summary>
        /// 根据元数据实例查找所有的元数据定义
        /// </summary>
        /// <param name="metaEntity">元数据实例</param>
        /// <returns></returns>
        List<RECP_BO_ProMetaDefineModel> GetProMetaDefineByMetaEntity(String metaEntity);
        /// <summary>
        /// 专业元数据结构定义 更新操作
        /// </summary>
        /// <param name="fieldNo">标识符</param>
        /// <param name="fieldCode">字段编码</param>
        /// <param name="fieldCaption">字段标题</param>
        void UpdateMetaTree(Guid fieldNo,string fieldCode,string fieldCaption);
        /// <summary>
        /// 专业元数据结构定义树 用户添加一个树节点
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="parentId">父ID</param>
        /// <param name="text">当前节点的text</param>
        /// <param name="fieldNo">标识符</param>
        /// <param name="metaEntity">元数据实体</param>
        /// <returns></returns>
        string SaveMetaDataTree(string state, Guid parentId, string text, Guid fieldNo, string metaEntity);
        /// <summary>
        /// 拿到RECP_BO_ProMetaDefine 的FieldCaption值
        /// </summary>
        /// <param name="metaEntity">元数据实体 targetTree的value</param>
        /// <param name="fieldCode">字段编码</param>
        /// <returns></returns>
        List<RECP_BO_ProMetaDefineModel> GetFieldCaption(string metaEntity, string fieldCode);
    }
}
