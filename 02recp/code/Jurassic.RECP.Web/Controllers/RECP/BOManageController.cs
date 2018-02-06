using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Service;
using Jurassic.RECP.DataService.Tool;
using Jurassic.WebFrame;
using NPOI.OpenXmlFormats.Dml.Chart;
using System.IO;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    [Authorize]
    public class BOManageController : BaseController
    {
        /// <summary>
        /// 服务层接口
        /// </summary>
        private IBO_BaseInfoService _BO_BaseInfoService { get; set; }
        private IBO_BOAliasService _BO_BOAliasService { get; set; }
        private IGT_CodeTypeService GT_CodeTypeService { get; set; }
        private IGT_CodeService GT_CodeService { get; set; }
        private IRECP_ScopeBaseInfoService recp_ScopeBaseInfoService { get; set; }

        /// <summary>
        /// 服务层接口构造方法注入
        /// </summary>
        /// <param name="_IBO_BaseInfoService"></param>
        /// <param name="_IBO_BOAliasService"></param>
        /// <param name="_IGT_CodeTypeService"></param>
        /// <param name="_IGT_CodeService"></param>
        /// <param name="_IRECP_ScopeBaseInfoService"></param>
        public BOManageController(IBO_BaseInfoService _IBO_BaseInfoService, IBO_BOAliasService _IBO_BOAliasService, IGT_CodeTypeService _IGT_CodeTypeService, IGT_CodeService _IGT_CodeService, IRECP_ScopeBaseInfoService _IRECP_ScopeBaseInfoService)
        {
            _BO_BaseInfoService = _IBO_BaseInfoService;
            _BO_BOAliasService = _IBO_BOAliasService;
            GT_CodeTypeService = _IGT_CodeTypeService;
            GT_CodeService = _IGT_CodeService;
            recp_ScopeBaseInfoService = _IRECP_ScopeBaseInfoService;
        }

        /// <summary>
        /// 目标对象页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetBOBaseInfo(string type, string id = "")
        {
            if (type == "辖区范围")
            {
                var list = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel { BOC = type, ID = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : Guid.Empty }).Select(x => new RECP_ScopeBaseInfoModel { ID = x.ID, Name = x.Name, PID = x.PID, BOC = x.BOC, BOT = x.BOT, Remark = x.Remark, SID = x.SID });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = type, ID = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : Guid.Empty }).Select(x => new BO_BaseInfoModel { ID = x.ID, Name = x.Name, PID = x.PID, BOC = x.BOC, BOT = x.BOT, Remark = x.Remark, SID = x.SID, OrderIndex = x.OrderIndex }).OrderBy(x => x.OrderIndex);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取对象特征信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetBOFeatureInfo(string type, string id = "")
        {
            var strXml = "";
            var model = new BO_BaseFeatureModel();
            if (type == "辖区范围")
            {
                var infos = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel
                {
                    BOC = type,
                    ID = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : Guid.Empty
                });
                foreach (var info in infos)
                {
                    strXml = info.ObjectParam != null ? info.ObjectParam : "";
                }
            }
            else
            {
                var infos = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel
                {
                    BOC = type,
                    ID = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : Guid.Empty
                });
                foreach (var info in infos)
                {
                    strXml = info.ObjectParam != null ? info.ObjectParam : "";
                }
            }

            if (!string.IsNullOrWhiteSpace(strXml))
            {
                var xRoot = XElement.Parse(strXml);
                var tempFeature = xRoot.Elements().Where(x => x.Name.LocalName == "Feature");
                if (tempFeature.FirstOrDefault() != null)
                {
                    var tempReserve = tempFeature.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Reserve");
                    var tempWell = tempFeature.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Well");
                    #region Reserve Param
                    if (tempReserve.FirstOrDefault() != null)
                    {
                        var tempGeoMap = tempReserve.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "GeoMap");
                        var attGeoMap = tempGeoMap.Attributes();
                        foreach (var att in attGeoMap)
                        {
                            if (att.Name == "url")
                            {
                                model.GeoMapUrl = att.Value;
                            }
                        }
                        if (tempGeoMap.FirstOrDefault() != null)
                        {
                            var tempLayer = tempGeoMap.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Layer");
                            var attLayer = tempLayer.Attributes();
                            foreach (var att in attLayer)
                            {
                                if (att.Name == "name")
                                {
                                    model.LayerName = att.Value;
                                }
                            }
                            if (tempLayer.FirstOrDefault() != null)
                            {
                                var tempElement = tempLayer.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Element");
                                var attElement = tempElement.Attributes();
                                foreach (var att in attElement)
                                {
                                    if (att.Name == "name")
                                    {
                                        model.ElementName = att.Value;
                                    }
                                    if (att.Name == "id")
                                    {
                                        model.ElementId = att.Value;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Well Param
                    if (tempWell.FirstOrDefault() != null)
                    {
                        var tempGeoMap = tempWell.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "GeoMap");
                        var attGeoMap = tempGeoMap.Attributes();
                        foreach (var att in attGeoMap)
                        {
                            if (att.Name == "url")
                            {
                                model.GeoMapUrl2 = att.Value;
                            }
                        }
                        if (tempGeoMap.FirstOrDefault() != null)
                        {
                            var tempLayer = tempGeoMap.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Layer");
                            var attLayer = tempLayer.Attributes();
                            foreach (var att in attLayer)
                            {
                                if (att.Name == "name")
                                {
                                    model.LayerName2 = att.Value;
                                }
                            }
                            if (tempLayer.FirstOrDefault() != null)
                            {
                                var tempElement = tempLayer.FirstOrDefault().Elements().Where(x => x.Name.LocalName == "Element");
                                var attElement = tempElement.Attributes();
                                foreach (var att in attElement)
                                {
                                    if (att.Name == "name")
                                    {
                                        model.ElementName2 = att.Value;
                                    }
                                    if (att.Name == "id")
                                    {
                                        model.ElementId2 = att.Value;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存对象属性
        /// </summary>
        /// <param name="nodeParams"></param>
        /// <param name="boc"></param> 
        public void SaveBOBaseInfo(string nodeParams, string boc)
        {
            List<BO_BaseInfoModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<BO_BaseInfoModel>)) as List<BO_BaseInfoModel>;
            if (models != null && models.Count > 0)
            {
                foreach (var m in models)
                {
                    m.CreatedBy = CurrentUser.Name;
                    m.BOC = boc;
                    if (m._pid == -1)
                    {
                        m.PID = null;
                    }
                    m.ID = Guid.NewGuid();
                }
                foreach (var m in models)
                {
                    if (m.PID == null && m._pid != -1)
                    {
                        BO_BaseInfoModel model = models.Find(delegate (BO_BaseInfoModel bo) { return bo._id == m._pid; });
                        if (model != null)
                        {
                            m.PID = model.ID;
                        }
                    }
                }
                _BO_BaseInfoService.ModifyBO_BaseInfo(models);
            }
        }

        /// <summary>
        /// 新增bo属性（添加）
        /// </summary>
        /// <param name="nodeParams"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public string AddBOBaseInfo(string nodeParams, string boc)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            if (boc == "辖区范围")
            {
                List<RECP_ScopeBaseInfoModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<RECP_ScopeBaseInfoModel>)) as List<RECP_ScopeBaseInfoModel>;
                if (models != null && models.Count == 1)
                {
                    Guid newId = Guid.NewGuid();
                    foreach (var m in models)
                    {
                        m.CreatedBy = CurrentUser.Name;
                        m.BOC = boc;
                        m.ID = newId;
                    }
                    recp_ScopeBaseInfoService.ModifyScopeBaseInfo(models);
                    return newId.ToString();
                }
            }
            else
            {
                List<BO_BaseInfoModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<BO_BaseInfoModel>)) as List<BO_BaseInfoModel>;
                if (models != null && models.Count == 1)
                {
                    Guid newId = Guid.NewGuid();
                    foreach (var m in models)
                    {
                        m.CreatedBy = CurrentUser.Name;
                        m.BOC = boc;
                        m.ID = newId;
                    }
                    _BO_BaseInfoService.ModifyBO_BaseInfo(models);
                    return newId.ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 获取 name 在bo中同名数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetBOInfoNameCount(string name, string type)
        {
            name = HttpUtility.UrlDecode(name);
            type = HttpUtility.UrlDecode(type);
            if (type == "辖区范围")
            {
                var info = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel { Name = name }).ToList();
                return Json(info.Count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var info = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { Name = name }).ToList();
                return Json(info.Count, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新bo属性（更新，删除）
        /// </summary>
        /// <param name="nodeParams"></param>
        /// <param name="boc"></param>
        public void UpdateBOBaseInfo(string nodeParams, string boc)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            if (boc == "辖区范围")
            {
                List<RECP_ScopeBaseInfoModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<RECP_ScopeBaseInfoModel>)) as List<RECP_ScopeBaseInfoModel>;
                if (models != null && models.Count > 0)
                {
                    foreach (var m in models)
                    {
                        m.CreatedBy = CurrentUser.Name;
                        m.BOC = boc;
                    }
                    recp_ScopeBaseInfoService.ModifyScopeBaseInfo(models);
                }
            }
            else
            {
                List<BO_BaseInfoModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<BO_BaseInfoModel>)) as List<BO_BaseInfoModel>;
                if (models != null && models.Count > 0)
                {
                    foreach (var m in models)
                    {
                        m.CreatedBy = CurrentUser.Name;
                        m.BOC = boc;
                    }
                    _BO_BaseInfoService.ModifyBO_BaseInfo(models);
                }
            }
        }

        /// <summary>
        /// 获取 name,boc 在bo中同名数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public JsonResult GetCountByNameBoc(string name, string boc)
        {
            name = HttpUtility.UrlDecode(name);
            boc = HttpUtility.UrlDecode(boc);
            var info = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { Name = name, BOC = boc }).ToList();
            return Json(info.Count, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取对象别名列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetBOAlias(string id)
        {
            if (id == null) return Json("", JsonRequestBehavior.AllowGet);
            return Json(_BO_BOAliasService.GetBOAlias(new Guid(id)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取码表Id信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult GetGT_CodeTypeInfo(string code)
        {
            return Json(GT_CodeTypeService.GetGT_CodeType(new GT_CodeTypeModel()
            {
                Code = code
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取码表属性信息列表
        /// </summary>
        /// <param name="id">CodeTypeId</param>
        /// <returns></returns>
        public JsonResult GetGT_CodeInfo(string id)
        {
            return Json(GT_CodeService.GetGT_CodeInfo(new GT_CodeModel()
            {
                CodeTypeId = Convert.ToInt32(id)
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存对象属性，别名属性
        /// </summary>
        /// <param name="boc"></param>
        /// <param name="id"></param>
        /// <param name="baseInfo"></param>
        /// <param name="alias"></param>
        /// <param name="baseFeature"></param>
        public void SaveBOInfo(string boc, string id, string baseInfo, string alias, string baseFeature)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (boc == "辖区范围")
                {
                    RECP_ScopeBaseInfoModel baseModel = JsonUtil.JsonToObject(baseInfo, typeof(RECP_ScopeBaseInfoModel)) as RECP_ScopeBaseInfoModel;
                    BO_BaseFeatureModel baseFeatureModel = JsonUtil.JsonToObject(baseFeature, typeof(BO_BaseFeatureModel)) as BO_BaseFeatureModel;
                    if (baseModel != null)
                    {
                        baseModel.ID = new Guid(id);
                        baseModel.CreatedBy = CurrentUser.Name;
                        baseModel.BOC = boc;
                        recp_ScopeBaseInfoService.UpdateScopeBaseInfo(baseModel, baseFeatureModel);
                    }
                }
                else
                {
                    BO_BaseInfoModel baseInfoModel = JsonUtil.JsonToObject(baseInfo, typeof(BO_BaseInfoModel)) as BO_BaseInfoModel;
                    BO_BaseFeatureModel baseFeatureModel = JsonUtil.JsonToObject(baseFeature, typeof(BO_BaseFeatureModel)) as BO_BaseFeatureModel;
                    if (baseInfoModel != null)
                    {
                        baseInfoModel.ID = new Guid(id);
                        baseInfoModel.CreatedBy = CurrentUser.Name;
                        baseInfoModel.BOC = boc;
                        _BO_BaseInfoService.UpdateBO_BaseInfo(baseInfoModel, baseFeatureModel);
                    }
                }
                List<BO_BOAliasModel> aliasModels = JsonUtil.JsonToObject(alias, typeof(List<BO_BOAliasModel>)) as List<BO_BOAliasModel>;
                if (aliasModels != null)
                {
                    foreach (var m in aliasModels)
                    {
                        m.AliasID = m.AliasID == Guid.Empty ? Guid.NewGuid() : m.AliasID;
                        m.BOID = new Guid(id);
                        m.CreatedBy = CurrentUser.Name;
                    }
                    _BO_BOAliasService.ModifyBO_BOAlias(aliasModels);
                }
            }
        }

        /// <summary>
        /// 上传图件，对应bo
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadBoGdbFile()
        {
            var upLoadFile = Request.Files["Fdata"];
            if (upLoadFile == null) return Json("null");
            var name = upLoadFile.FileName;
            var fileName = name.Substring(name.LastIndexOf("\\") + 1);
            var filePath = Request.PhysicalApplicationPath + "GDB\\" + fileName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            else
            {
                System.IO.File.Delete(filePath);
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            upLoadFile.SaveAs(filePath);
            return Json(fileName);
        }

        /// <summary>
        /// 图件导入页
        /// </summary>
        /// <returns></returns>
        public ActionResult Importing()
        {
            return View();
        }
    }
}