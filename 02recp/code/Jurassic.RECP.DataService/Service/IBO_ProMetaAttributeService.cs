using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface IBO_ProMetaAttributeService
    {
        /// <summary>
        /// 通过元数据定义表的流水号Id拿到所有的元数据的属性
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <returns></returns>
        List<RECP_BO_ProMetaAttributeModel> GetProMetaAttributeByFieldNo(Guid fieldNo);
        /// <summary>
        /// 删除元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        void DeleteMetaAttribute(Guid fieldNo, string attrCode);
        /// <summary>
        /// 更新元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        void UpdateMetaAttribute(Guid fieldNo, string attrCode, string attrCaption, string attrValue, int attrIndex);
        /// <summary>
        /// 增加元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        void AddMetaAttribute(Guid fieldNo, string attrCode, string attrCaption, string attrValue, int attrIndex);
        /// <summary>
        /// MetaAttribute 表格所有增删改查操作的保存方法
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="nodeParams">修改的表格数据</param>
        void SaveMetadata(Guid fieldNo,string nodeParams);
    }
}
