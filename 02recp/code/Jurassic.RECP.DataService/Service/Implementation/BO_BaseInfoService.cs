using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Service;

namespace Jurassic.RECP.DataService.Service
{
    public class BO_BaseInfoService : IBO_BaseInfoService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IBO_BaseInfoEFRepository BO_BaseInfoEFRepository { get; set; }
        private IBO_BOAliasEFRepository BO_BOAliasEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="_IBO_BaseInfoEFRepository"></param>
        /// <param name="_IBO_BOAliasEFRepository"></param>
        public BO_BaseInfoService(IBO_BaseInfoEFRepository _IBO_BaseInfoEFRepository, IBO_BOAliasEFRepository _IBO_BOAliasEFRepository)
        {
            BO_BaseInfoEFRepository = _IBO_BaseInfoEFRepository;
            BO_BOAliasEFRepository = _IBO_BOAliasEFRepository;
        }

        /// <summary>
        /// 获取BO属性
        /// </summary>
        /// <param name="model">BO_BaseInfoModel</param>
        /// <returns></returns>
        public IEnumerable<BO_BaseInfoModel> GetBO_BaseInfo(BO_BaseInfoModel model)
        {
            IEnumerable<BO_BaseInfo> info = BO_BaseInfoEFRepository.GetQuery();
            if (model.ID.ToString() != Guid.Empty.ToString())
            {
                info = info.Where(x => x.ID == model.ID);
            }
            if (!string.IsNullOrWhiteSpace(model.PID.ToString()))
            {
                info = info.Where(x => x.PID == model.PID);
            }
            if (!string.IsNullOrWhiteSpace(model.BOT))
            {
                info = info.Where(x => x.BOT == model.BOT);
            }
            if (!string.IsNullOrWhiteSpace(model.BOC))
            {
                info = info.Where(x => x.BOC == model.BOC);
            }
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                info = info.Where(x => x.Name == model.Name);
            }
            return info.Select(Mapper.Map<BO_BaseInfo, BO_BaseInfoModel>).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// 编辑BO属性
        /// </summary>
        /// <param name="models"></param>
        public void ModifyBO_BaseInfo(List<BO_BaseInfoModel> models)
        {
            foreach (var model in models)
            {
                if (model._state == EnumNodeState.added.ToString())
                {
                    AddBO_BaseInfoByTree(model);
                }
                if (model._state == EnumNodeState.modified.ToString())
                {
                    UpdateBO_BaseInfoByTree(model);
                }
            }
            if (models[0]._state == EnumNodeState.removed.ToString())
            {
                RemoveBO_AliasByTree(models);
                RemoveBO_BaseInfoByTree(models);
            }
        }

        /// <summary>
        /// BO属性添加，对象树操作
        /// </summary>
        /// <param name="model"></param>
        public void AddBO_BaseInfoByTree(BO_BaseInfoModel model)
        {
            BO_BaseInfoEFRepository.Add(new BO_BaseInfo
            {
                ID = model.ID,
                PID = model.PID,
                BOC = model.BOC,
                Name = model.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
            });
        }

        /// <summary>
        /// BO特定属性更新，对象树操作
        /// </summary>
        /// <param name="model"></param>
        public void UpdateBO_BaseInfoByTree(BO_BaseInfoModel model)
        {
            var baseInfo = BO_BaseInfoEFRepository.Find(t => t.ID == model.ID);
            if (baseInfo == null) return;
            baseInfo.Name = model.Name;
            baseInfo.PID = model.PID;
            baseInfo.LastUpdatedBy = model.CreatedBy;
            baseInfo.LastUpdatedDate = DateTime.Now;
            BO_BaseInfoEFRepository.Update(baseInfo);
        }

        /// <summary>
        /// BO 别名删除
        /// </summary>
        /// <param name="models"></param>
        private void RemoveBO_AliasByTree(List<BO_BaseInfoModel> models)
        {
            foreach (var model in models)
            {
                var alias = BO_BOAliasEFRepository.Find(t => t.BOID == model.ID);
                if (alias != null)
                {
                    BO_BOAliasEFRepository.DeleteList(x => x.BOID == model.ID);
                }
            }
        }

        /// <summary>
        /// BO特定属性删除，对象树操作
        /// </summary>
        /// <param name="model"></param>
        public void RemoveBO_BaseInfoByTree(List<BO_BaseInfoModel> models)
        {
            foreach (var model in models)
            {
                var baseInfo = BO_BaseInfoEFRepository.Find(t => t.ID == model.ID);
                if (baseInfo != null)
                {
                    BO_BaseInfoEFRepository.Delete(baseInfo);
                }
            }
        }

        /// <summary>
        /// BO特定属性更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateBO_BaseInfo(BO_BaseInfoModel model, BO_BaseFeatureModel baseFeatureModel)
        {
            var baseInfo = BO_BaseInfoEFRepository.Find(t => t.ID == model.ID);
            if (baseInfo == null) return;

            var strXml = baseInfo.ObjectParam;
            baseInfo.ObjectParam = GetObjectParam(strXml, baseFeatureModel);

            baseInfo.Name = model.Name;
            baseInfo.BOC = model.BOC;
            baseInfo.BOT = model.BOT;
            baseInfo.SID = model.SID;
            baseInfo.Remark = model.Remark;
            baseInfo.LastUpdatedBy = model.CreatedBy;
            baseInfo.LastUpdatedDate = DateTime.Now;
            BO_BaseInfoEFRepository.Update(baseInfo);
        }

        private string GetObjectParam(string strObjectParam, BO_BaseFeatureModel model)
        {
            var strXmlData = "<XmlData></XmlData>";
            var xRoot = XElement.Parse(strXmlData);
            if (string.IsNullOrWhiteSpace(strObjectParam))
            {
                //add
                var xFeatureNode= XElement.Parse(GetFeatureXml(model));
                xRoot.Add(xFeatureNode);
                return xRoot.ToString();
            }
            else
            {
                // hasFeature?
                XmlDocument xmlD = new XmlDocument();
                xmlD.LoadXml(strObjectParam);
                XmlElement xmlE = xmlD.DocumentElement;
                var hasFeatureNode = xmlE.SelectSingleNode("/XmlData/Feature") != null;

                var xRoot2 = XElement.Parse(strObjectParam);
                var xFeatureNode = XElement.Parse(GetFeatureXml(model));
                if (hasFeatureNode)
                {
                    //remove add Feature node
                    xRoot2.Elements().Where(x => x.Name.LocalName == "Feature").Remove();
                    xRoot2.Add(xFeatureNode); 
                }
                else
                {
                    //add Feature node (此情况理论上不存在)
                    xRoot2.Add(xFeatureNode);
                }
                return xRoot2.ToString();
            }  
        }

        private string GetFeatureXml(BO_BaseFeatureModel model)
        {
            var result = "";
            result += "<Feature>";
            result += "<Reserve>";
            result += "<GeoMap url=\""+model.GeoMapUrl+"\">";
            result += "<Layer name=\"" + model.LayerName + "\">";
            result += "<Element name=\"" + model.ElementName + "\" id=\"" + model.ElementId + "\">";
            result += "</Element>";
            result += "</Layer>";
            result += "</GeoMap>";
            result += "</Reserve>";
            result += "<Well>";
            result += "<GeoMap url=\"" + model.GeoMapUrl2 + "\">";
            result += "<Layer name=\"" + model.LayerName2 + "\">";
            result += "<Element name=\"" + model.ElementName2 + "\" id=\"" + model.ElementId2 + "\">";
            result += "</Element>";
            result += "</Layer>";
            result += "</GeoMap>";
            result += "</Well>";
            result += "</Feature>";
            return result;
        }
    }
}
