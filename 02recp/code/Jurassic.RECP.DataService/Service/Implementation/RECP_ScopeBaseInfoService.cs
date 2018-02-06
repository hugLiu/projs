using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Jurassic.RECP.DataService.Service
{
    public class RECP_ScopeBaseInfoService : IRECP_ScopeBaseInfoService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IRECP_ScopeBaseInfoEFRepository recp_ScopeBaseInfoEFRepository { get; set; }
        private IBO_BOAliasEFRepository BO_BOAliasEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="iRECP_ScopeBaseInfoEFRepository"></param>
        /// <param name="_BO_BOAliasEFRepository"></param>
        public RECP_ScopeBaseInfoService(IRECP_ScopeBaseInfoEFRepository iRECP_ScopeBaseInfoEFRepository, IBO_BOAliasEFRepository _BO_BOAliasEFRepository)
        {
            recp_ScopeBaseInfoEFRepository = iRECP_ScopeBaseInfoEFRepository;
            BO_BOAliasEFRepository = _BO_BOAliasEFRepository;
        }

        /// <summary>
        /// 获取BO属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<RECP_ScopeBaseInfoModel> GetScopeBaseInfo(RECP_ScopeBaseInfoModel model)
        {
            IEnumerable<RECP_ScopeBaseInfo> info = recp_ScopeBaseInfoEFRepository.GetQuery();
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
            return info.Select(Mapper.Map<RECP_ScopeBaseInfo, RECP_ScopeBaseInfoModel>).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// 编辑属性
        /// </summary>
        /// <param name="models"></param>
        public void ModifyScopeBaseInfo(List<RECP_ScopeBaseInfoModel> models)
        {
            foreach (var model in models)
            {
                if (model._state == EnumNodeState.added.ToString())
                {
                    AddScopeBaseInfoByTree(model);
                }
                if (model._state == EnumNodeState.modified.ToString())
                {
                    UpdateScopeBaseInfoByTree(model);
                }
            }
            if (models[0]._state == EnumNodeState.removed.ToString())
            {
                RemoveBO_AliasByTree(models);
                RemoveScopeBaseInfoByTree(models);
            }
        }

        /// <summary>
        /// 属性添加，对象树操作
        /// </summary>
        /// <param name="model"></param>
        public void AddScopeBaseInfoByTree(RECP_ScopeBaseInfoModel model)
        {
            recp_ScopeBaseInfoEFRepository.Add(new RECP_ScopeBaseInfo
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
        /// 属性更新，对象树操作
        /// </summary>
        /// <param name="model"></param>
        public void UpdateScopeBaseInfoByTree(RECP_ScopeBaseInfoModel model)
        {
            var baseInfo = recp_ScopeBaseInfoEFRepository.Find(t => t.ID == model.ID);
            if (baseInfo == null) return;
            baseInfo.Name = model.Name;
            baseInfo.PID = model.PID;
            baseInfo.BOC = model.BOC;
            baseInfo.LastUpdatedBy = model.CreatedBy;
            baseInfo.LastUpdatedDate = DateTime.Now;
            recp_ScopeBaseInfoEFRepository.Update(baseInfo);
        }

        /// <summary>
        /// 属性别名删除
        /// </summary>
        /// <param name="models"></param>
        private void RemoveBO_AliasByTree(List<RECP_ScopeBaseInfoModel> models)
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
        public void RemoveScopeBaseInfoByTree(List<RECP_ScopeBaseInfoModel> models)
        {
            foreach (var model in models)
            {
                var baseInfo = recp_ScopeBaseInfoEFRepository.Find(t => t.ID == model.ID);
                if (baseInfo != null)
                {
                    recp_ScopeBaseInfoEFRepository.Delete(baseInfo);
                }
            }
        }

        /// <summary>
        /// BO特定属性更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateScopeBaseInfo(RECP_ScopeBaseInfoModel model, BO_BaseFeatureModel baseFeatureModel)
        {
            var baseInfo = recp_ScopeBaseInfoEFRepository.Find(t => t.ID == model.ID);
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
            recp_ScopeBaseInfoEFRepository.Update(baseInfo);
        }

        private string GetObjectParam(string strObjectParam, BO_BaseFeatureModel model)
        {
            var strXmlData = "<XmlData></XmlData>";
            var xRoot = XElement.Parse(strXmlData);
            if (string.IsNullOrWhiteSpace(strObjectParam))
            {
                //add
                var xFeatureNode = XElement.Parse(GetFeatureXml(model));
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
            result += "<GeoMap url=\"" + model.GeoMapUrl + "\">";
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
