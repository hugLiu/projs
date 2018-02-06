using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataService.Tool;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class BO_ProMetaAttributeService : IBO_ProMetaAttributeService
    {
        private IBO_ProMetaAttributeEFRepository ProMetaAttribute { get; set; }
        public BO_ProMetaAttributeService(IBO_ProMetaAttributeEFRepository proMetaAttribute)
        {
            ProMetaAttribute = proMetaAttribute;
        }
        /// <summary>
        /// 通过元数据定义表的流水号Id拿到所有的元数据的属性
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <returns></returns>
        public List<RECP_BO_ProMetaAttributeModel> GetProMetaAttributeByFieldNo(Guid fieldNo)
        {
            var result = ProMetaAttribute.GetQuery().Where(t => t.FieldNo == fieldNo).ToList().OrderBy(m=>m.AttrIndex);
            return result.Select(AutoMapper.Mapper.Map<RECP_BO_ProMetaAttribute, RECP_BO_ProMetaAttributeModel>).ToList();
        }
        /// <summary>
        /// 删除元数据属性表中的某一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        public void DeleteMetaAttribute(Guid fieldNo, string attrCode)
        {
            var result = ProMetaAttribute.Find(t => t.FieldNo == fieldNo && t.AttrCode == attrCode);
            if (result == null) return;
            ProMetaAttribute.Delete(result);
        }
        /// <summary>
        /// 更新元数据属性表中的某一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        public void UpdateMetaAttribute(Guid fieldNo, string attrCode, string attrCaption, string attrValue, int attrIndex)
        {
            var result = ProMetaAttribute.Find(t => t.FieldNo == fieldNo && t.AttrCode == attrCode);
            result.AttrCode = attrCode;
            result.AttrCaption = attrCaption;
            result.AttrValue = attrValue;
            result.AttrIndex = attrIndex;
            ProMetaAttribute.Update(result);
        }
        /// <summary>
        /// 增加元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        public void AddMetaAttribute(Guid fieldNo, string attrCode, string attrCaption, string attrValue, int attrIndex)
        {
            var result = new RECP_BO_ProMetaAttribute
            {
                FieldNo = fieldNo,
                AttrCode = attrCode,
                AttrCaption = attrCaption,
                AttrValue = attrValue,
                AttrIndex = attrIndex,
                State=1
               // CreatedDate = DateTime.Now
            };
            ProMetaAttribute.Add(result);
        }
        /// <summary>
        /// 保存元数据
        /// </summary>
        /// <param name="fieldNo"></param>
        /// <param name="nodeParams"></param>
        public void SaveMetadata(Guid fieldNo,string nodeParams)
        {
            List<RECP_BO_ProMetaAttributeModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<RECP_BO_ProMetaAttributeModel>)) as List<RECP_BO_ProMetaAttributeModel>;
            if (models != null)
            {
                for(var i = 0; i < models.Count; i++)
                {
                    if (models[i]._state == "added")
                    {
                        string _attrCode = models[i].AttrCode;
                        string _attrCaption = models[i].AttrCaption;
                        string _attrValue = models[i].AttrValue;
                        int _attrIndex = models[i].AttrIndex;
                        var result = new RECP_BO_ProMetaAttribute
                        {
                            FieldNo = fieldNo,
                            AttrCode = _attrCode,
                            AttrCaption = _attrCaption,
                            AttrValue = _attrValue,
                            AttrIndex = _attrIndex,
                            State = 1,
                            CreatedDate = DateTime.Now
                        };
                        ProMetaAttribute.Add(result);
                    }
                    else if(models[i]._state == "modified")
                    {
                        string _attrCode = models[i].AttrCode;

                        var result = ProMetaAttribute.Find(t => t.FieldNo == fieldNo && t.AttrCode == _attrCode && t.State==1);
                        if (result != null)
                        {
                            result.AttrCode = models[i].AttrCode;
                            result.AttrCaption = models[i].AttrCaption;
                            result.AttrValue = models[i].AttrValue;
                            result.AttrIndex = models[i].AttrIndex;
                            ProMetaAttribute.Update(result);
                        }
                    }
                    else if(models[i]._state== "removed")
                    {
                        string _attrCode = models[i].AttrCode;
                        var result = ProMetaAttribute.Find(t => t.FieldNo == fieldNo && t.AttrCode == _attrCode && t.State == 1);
                        if (result == null) return;
                        ProMetaAttribute.Delete(result);
                    }
                }
            }
            else
            {
                return;
            }
        } 
    }
}
