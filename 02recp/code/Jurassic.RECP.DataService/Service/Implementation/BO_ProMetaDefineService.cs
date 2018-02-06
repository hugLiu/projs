using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataService.Tool;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class BO_ProMetaDefineService : IBO_ProMetaDefineService
    {
        private IBO_ProMetaDefineEFRepository ProMetaDefine { get; set; }
        private IBO_ProMetaAttributeEFRepository ProMetaAttribute { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="proMetaDefine">专业元数据定义</param>
        /// <param name="proMetaAttribute">专业元数据属性</param>
        public BO_ProMetaDefineService(IBO_ProMetaDefineEFRepository proMetaDefine, IBO_ProMetaAttributeEFRepository proMetaAttribute)
        {
            ProMetaDefine = proMetaDefine;
            ProMetaAttribute = proMetaAttribute;
        }
        /// <summary>
        /// 根据元数据实例查找所有的元数据定义
        /// </summary>
        /// <param name="metaEntity">元数据实例</param>
        /// <returns></returns>
        public List<RECP_BO_ProMetaDefineModel> GetProMetaDefineByMetaEntity(string metaEntity)
        {
            var result = ProMetaDefine.GetQuery().Where(t => t.MetaEntity == metaEntity).ToList().OrderBy(m=>m.FieldIndex);
            return result.Select(AutoMapper.Mapper.Map<RECP_BO_ProMetaDefine,RECP_BO_ProMetaDefineModel>).ToList();
        }
        /// <summary>
        /// 专业元数据结构定义 更新树节点的操作
        /// </summary>
        /// <param name="fieldNo"></param>
        /// <param name="fieldCode"></param>
        /// <param name="fieldCaption"></param>
        public void UpdateMetaTree(Guid fieldNo, string fieldCode, string fieldCaption)
        {
            var result = ProMetaDefine.Find(t => t.FieldNo == fieldNo);
            result.FieldCode = fieldCode;
            result.FieldCaption = fieldCaption;
            ProMetaDefine.Update(result);
        }
        /// <summary>
        /// 专业元数据结构定义 添加节点和删除节点的操作
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="parentId">父ID</param>
        /// <param name="text">当前树节点text</param>
        /// <param name="fieldNo">标识符</param>
        /// <param name="metaEntity">元数据实体</param>
        /// <returns></returns>
        public string SaveMetaDataTree(string state, Guid parentId, string text,Guid fieldNo,string metaEntity)
        {
            if (state == "added")
            {
                var count = ProMetaDefine.GetQuery().Where(m => m.MetaEntity == metaEntity).Max(t => t.FieldIndex);
                var result = new RECP_BO_ProMetaDefine
                {
                    FieldNo = Guid.NewGuid(),
                    MetaEntity = metaEntity,
                    FieldCode = text,
                    FieldCaption=text,
                    ParentNo= parentId,
                    FieldIndex= count+1,
                    CreatedDate=DateTime.Now,
                    State=1

                };
                ProMetaDefine.Add(result);
                return "";
            }
            else if (state == "removed")
            {
                var result = ProMetaAttribute.GetQuery().Where(t => t.FieldNo == fieldNo).ToList().Count();
                if (result > 0)
                {
                    return "attIsNotNull";
                }
                else
                {
                    var result2 = ProMetaDefine.Find(t => t.FieldNo == fieldNo);
                    ProMetaDefine.Delete(result2);
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// 专业元数据结构定义 tooltip 拿到FieldCaption
        /// </summary>
        /// <param name="metaEntity">元数据实体</param>
        /// <param name="fieldCode">字段编码</param>
        /// <returns></returns>
        public List<RECP_BO_ProMetaDefineModel> GetFieldCaption(string metaEntity, string fieldCode)
        {
            var result = ProMetaDefine.GetQuery().Where(x => x.MetaEntity == metaEntity && x.FieldCode == fieldCode).ToList();
            return result.Select(AutoMapper.Mapper.Map<RECP_BO_ProMetaDefine, RECP_BO_ProMetaDefineModel>).ToList();
        }
    }
}
