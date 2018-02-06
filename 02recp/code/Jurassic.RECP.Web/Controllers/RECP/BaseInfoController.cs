using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Service;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using Jurassic.RECP.DataService.Models.View;
using Jurassic.WebFrame;
using Jurassic.AppCenter;
using Microsoft.Ajax.Utilities;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class BaseInfoController : BaseController
    {
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"]
            + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];
        private readonly string _achieveDown = System.Configuration.ConfigurationManager.AppSettings["AchieveDown"];
        /// <summary>
        /// 服务层接口
        /// </summary>
        private IBO_BaseInfoService _BO_BaseInfoService { get; set; }
        private IBO_ThreeLevReserveService _BO_ThreeLevReserveService { get; set; }
        private IBO_WellPointService _BO_WellPointService { get; set; }
        private IBO_WellTestService _BO_WellTestService { get; set; }
        private IRECP_ScopeBaseInfoService recp_ScopeBaseInfoService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_IBO_BaseInfoService">基础信息表</param>
        /// <param name="_IBO_ThreeLevReserveService">三级储量</param>
        /// <param name="_IBO_WellPointService">井点分布</param>
        public BaseInfoController(IBO_BaseInfoService _IBO_BaseInfoService, IBO_ThreeLevReserveService _IBO_ThreeLevReserveService, IBO_WellPointService _IBO_WellPointService, IBO_WellTestService _IBO_WellTestService, IRECP_ScopeBaseInfoService _IRECP_ScopeBaseInfoService)
        {
            _BO_BaseInfoService = _IBO_BaseInfoService;
            _BO_ThreeLevReserveService = _IBO_ThreeLevReserveService;
            _BO_WellPointService = _IBO_WellPointService;
            _BO_WellTestService = _IBO_WellTestService;
            recp_ScopeBaseInfoService = _IRECP_ScopeBaseInfoService;
        }

        /// <summary>
        /// 辖区范围分布
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeDistrbution()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }

        /// <summary>
        /// 三维地震分布
        /// </summary>
        /// <returns></returns>
        public ActionResult ThreeDSeismicDistrbution()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }

        /// <summary>
        /// 三级储量分布
        /// </summary>
        /// <returns></returns>
        public ActionResult ThreeLevReserveDistrbution()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }

        /// <summary>
        /// 剩余出油气井点分布
        /// </summary> 
        public ActionResult RemainDistrbution()
        {
            var user = User.Identity.GetUserId();
            if (User == null || string.IsNullOrWhiteSpace(user))
            {
                return Redirect("/Account/Logout");
            }
            ViewBag._apiPath = _apiPath;
            ViewBag._achieveDown = _achieveDown;
            return View();
        }

        /// <summary>
        /// 本地化数据（三级储量--储量类别查看）
        /// </summary>
        /// <returns></returns>
        public JsonResult ThreeLevGetReserveTreeData()
        {
            var url = Server.MapPath("/") + ("/Local/RECP/Json/ThreeLev/reserve-tree.json");
            var d = JsonConvert.ToString(System.IO.File.ReadAllText(url));
            return Json(d, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过pid获取baseinfo
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public JsonResult GetBOBaseInfoByPId(string pid)
        {
            var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { PID = new Guid(pid) }).Select(x => new BO_BaseInfoModel { ID = x.ID, Name = x.Name, PID = x.PID });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过id获取baseinfo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetBOBaseInfoById(string id)
        {
            var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { ID = new Guid(id) }).Select(x => new BO_BaseInfoModel { ID = x.ID, Name = x.Name, PID = x.PID });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="boc"></param>
        /// <returns></returns>
        public JsonResult GetBOBaseInfo(string boc)
        {
            try
            {
                if (boc == "地层年代")
                {
                    return Json(_BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = boc }).OrderBy(x => x.OrderIndex), JsonRequestBehavior.AllowGet);
                }
                else if (boc == "成果导航")
                {
                    var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = boc }).Select(x => new BO_BaseInfoModel { ID = x.ID, PID = x.PID, Name = x.Name });
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else if (boc == "辖区范围")
                {
                    var list = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel { BOC = boc }).Select(x => new RECP_ScopeBaseInfoModel { ID = x.ID, PID = x.PID, Name = x.Name });
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(_BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = boc }), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取对象gdb url
        /// </summary>
        /// <param name="boc"></param>
        /// <param name="id"></param>
        /// <param name="feature">xml 储量或井点的节点特征</param>
        /// <returns></returns>
        public JsonResult GetBOBaseInfoUrl(string boc, string id, string feature)
        {
            try
            {
                var url = "";
                var strXml = "";
                if (boc == "辖区范围")
                {
                    var list = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel { BOC = boc, ID = new Guid(id) }).Select(x => new RECP_ScopeBaseInfoModel { ObjectParam = x.ObjectParam }).ToList();
                    strXml = list.Count > 0 ? list[0].ObjectParam : "";
                }
                else
                {
                    var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = boc, ID = new Guid(id) }).Select(x => new BO_BaseInfoModel { ObjectParam = x.ObjectParam }).ToList();
                    strXml = list.Count > 0 ? list[0].ObjectParam : "";
                }
                if (!string.IsNullOrWhiteSpace(strXml))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXml);
                    XmlElement root = doc.DocumentElement;
                    var nodeUrl = "/XmlData/Feature/" + feature + "/GeoMap";
                    XmlNode xn = root.SelectSingleNode(nodeUrl);
                    var attr = xn.Attributes;
                    for (var i = 0; i < attr.Count; i++)
                    {
                        var name = attr[i].Name;
                        if (name == "url")
                        {
                            url = attr[i].Value;
                        }
                    }
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectParam"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoObjectParam(string objectParam)
        {
            try
            {
                if (objectParam != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(objectParam);
                    XmlElement root = doc.DocumentElement;

                    var url = @"" + root.InnerText;
                    url = url.Replace(@"/", @"\\");
                    var path = Server.MapPath("/").Replace(@"\", @"\\");
                    url = path + url;
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// 获取子节点所有专业数据(三维地震工区)
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ActionResult GetBOBaseInfoProDataByPid(string pid)
        {
            try
            {
                var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { PID = new Guid(pid), BOC = "三维地震" });
                var nodeUrl = "/XmlData/ProData/ThreeDSeismics";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var strId = l.ID.ToString();
                    var strXml = l.ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前节点专业数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetBOBaseInfoProDataById(string id)
        {
            try
            {
                var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { ID = new Guid(id), BOC = "三维地震" });
                var nodeUrl = "/XmlData/ProData/ThreeDSeismics";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var strId = l.ID.ToString();
                    var strXml = l.ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields.Distinct().ToList(), _fields);
                jss.Serialize(fieldModel.Distinct().ToList(), _fieldModel);
                jss.Serialize(headTitles.Distinct().ToList(), _headTitles);
                jss.Serialize(records.Distinct().ToList(), _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前节点特征数据（geomap）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        public ActionResult GetBaseInfoFeatureById(string id, string feature, string boc = "")
        {
            string strXml = "", url = "", layerName = "", elementName = "", elementId = "";
            if (boc == "辖区范围")
            {
                var list = recp_ScopeBaseInfoService.GetScopeBaseInfo(new RECP_ScopeBaseInfoModel { BOC = boc, ID = new Guid(id) }).Select(x => new RECP_ScopeBaseInfoModel { ObjectParam = x.ObjectParam }).ToList();
                strXml = list.Count > 0 ? list[0].ObjectParam : "";
            }
            else
            {
                var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = boc, ID = new Guid(id) }).Select(x => new BO_BaseInfoModel { ObjectParam = x.ObjectParam }).ToList();
                strXml = list.Count > 0 ? list[0].ObjectParam : "";
            }
            if (!String.IsNullOrWhiteSpace(strXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXml);
                XmlElement root = doc.DocumentElement;
                XmlNode map = root.SelectSingleNode("/XmlData/Feature/" + feature + "/GeoMap");
                XmlNode layer = root.SelectSingleNode("/XmlData/Feature/" + feature + "/GeoMap/Layer");
                XmlNode element = root.SelectSingleNode("/XmlData/Feature/" + feature + "/GeoMap/Layer/Element");

                for (var i = 0; map.Attributes != null && i < map.Attributes.Count; i++)
                {
                    if (map.Attributes[i].Name == "url")
                    {
                        url = map.Attributes[i].Value;
                    }
                }
                for (var i = 0; layer.Attributes != null && i < layer.Attributes.Count; i++)
                {
                    if (layer.Attributes[i].Name == "name")
                    {
                        layerName = layer.Attributes[i].Value;
                    }
                }
                for (var i = 0; element.Attributes != null && i < element.Attributes.Count; i++)
                {
                    if (element.Attributes[i].Name == "name")
                    {
                        elementName = element.Attributes[i].Value;
                    }
                    if (element.Attributes[i].Name == "id")
                    {
                        elementId = element.Attributes[i].Value;
                    }
                }

            }
            return Content(url + "#" + layerName + "#" + elementName + "#" + elementId);
        }

        /// <summary>
        /// 特定格式xml解析
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldModel"></param>
        /// <param name="headTitles"></param>
        /// <param name="records"></param>
        /// <param name="nodeUrl"></param>
        /// <param name="strXml"></param>
        /// <param name="strId"></param>
        /// <param name="flag">防止重复的标记项</param>
        public void XmlParse(ref List<string> fields, ref List<string> fieldModel, ref List<Dictionary<string, string>> headTitles, ref List<Dictionary<string, string>> records, string nodeUrl, string strXml, string strId, bool flag)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strXml);
            XmlElement root = doc.DocumentElement;
            var nodes = root.SelectSingleNode(nodeUrl);
            var hasNode = nodes != null ? nodes.HasChildNodes ? nodes.HasChildNodes : false : false;
            if (!hasNode) return;
            XmlNodeList nodeList = nodes.ChildNodes;
            if (nodeList.Count > 0)
            {
                var inFlag = true;
                foreach (XmlNode xn in nodeList)
                {
                    var record = new Dictionary<string, string>();
                    for (var i = 0; i < xn.ChildNodes.Count; i++)
                    {
                        var currentNode = xn.ChildNodes[i];
                        if (currentNode.FirstChild != null &&
                            currentNode.FirstChild.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            #region 暂时两级判断  
                            var headTitle1 = new Dictionary<string, string>();
                            var titleText = "";
                            for (var k = 0; currentNode.Attributes != null && k < currentNode.Attributes.Count; k++)
                            {
                                if (currentNode.Attributes[k].Name == "caption")
                                {
                                    titleText = currentNode.Attributes[k].Value;
                                }
                            }
                            var numberOfColumns = currentNode.ChildNodes.Count.ToString();
                            var startColumnName = currentNode.FirstChild.Name;
                            if (flag && inFlag)
                            {
                                headTitle1.Add("numberOfColumns", numberOfColumns);
                                headTitle1.Add("titleText", titleText);
                                headTitle1.Add("startColumnName", startColumnName);
                                headTitles.Add(headTitle1);
                            }
                            var cNodeList = currentNode.ChildNodes;
                            foreach (XmlNode cxn in cNodeList)
                            {
                                var currentNode2 = cxn;
                                var isShow = true;
                                var str = "";
                                for (var m = 0;
                                    currentNode2.Attributes != null && m < currentNode2.Attributes.Count;
                                    m++)
                                {
                                    if (currentNode2.Attributes[m].Name == "caption")
                                    {
                                        str += currentNode2.Attributes[m].Value;
                                    }
                                    if (currentNode2.Attributes[m].Name == "units")
                                    {
                                        str += "<br/>" + currentNode2.Attributes[m].Value;
                                    }
                                    if (currentNode2.Attributes[m].Name == "display" &&
                                        currentNode2.Attributes[m].Value == "fasle")
                                    {
                                        isShow = false;
                                    }
                                }
                                if (isShow)
                                {
                                    var key = currentNode2.Name;
                                    var value = currentNode2.InnerText.Trim();
                                    if (flag && inFlag)
                                    {
                                        if (!fields.Contains("编号"))
                                        {
                                            fields.Add("编号");
                                        }
                                        fields.Add(str);
                                        if (!fieldModel.Contains("id"))
                                        {
                                            fieldModel.Add("id");
                                        }
                                        fieldModel.Add(key);
                                    }
                                    if (!record.ContainsKey("id"))
                                    {
                                        record.Add("id", strId);
                                    }
                                    record.Add(key, value);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            var isShow = true;
                            var str = "";
                            for (var m = 0; currentNode.Attributes != null && m < currentNode.Attributes.Count; m++)
                            {
                                if (currentNode.Attributes[m].Name == "caption")
                                {
                                    str += currentNode.Attributes[m].Value;
                                }
                                if (currentNode.Attributes[m].Name == "units")
                                {
                                    str += "<br/>" + currentNode.Attributes[m].Value;
                                }
                                if (currentNode.Attributes[m].Name == "display" &&
                                    currentNode.Attributes[m].Value == "false")
                                {
                                    isShow = false;
                                }
                            }
                            if (isShow)
                            {
                                var key = currentNode.Name;
                                var value = currentNode.InnerText.Trim();
                                if (flag && inFlag)
                                {
                                    if (!fields.Contains("编号"))
                                    {
                                        fields.Add("编号");
                                    }
                                    fields.Add(str);
                                    if (!fieldModel.Contains("id"))
                                    {
                                        fieldModel.Add("id");
                                    }
                                    fieldModel.Add(key);
                                }
                                if (!record.ContainsKey("id"))
                                {
                                    record.Add("id", strId);
                                }
                                record.Add(key, value);
                            }
                        }
                    }
                    records.Add(record);
                    inFlag = false;
                }
            }
        }

        /// <summary>
        /// 获取子节点所有内容 (三级储量) （table baseInfo）
        /// </summary> 
        /// <returns></returns>
        public ActionResult GetBaseInfoViewData()
        {
            try
            {
                //var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(new BO_ThreeLevReserveModel());  数据源改变
                var list =
                    _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { BOC = "三级储量计算单元" })
                        .Where(x => x.PID != null)
                        .ToList();
                var nodeUrl = "/XmlData/ProData/ThreeLevReserve";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var strId = l.ID.ToString();
                    var strXml = l.ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过bot筛选视图数据
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetBaseInfoViewDataByBOT(string bot, string name)
        {
            try
            {
                var model = new BO_ThreeLevReserveModel();
                if (bot == "系")
                {
                    model.Series = name;
                }
                else if (bot == "组")
                {
                    model.Formation = name;
                }
                else if (bot == "层")
                {
                    model.Layer = name;
                }
                var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).DistinctBy(x => x.ID).ToList();
                var nodeUrl = "/XmlData/ProData/ThreeLevReserve";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var id = l.ID;
                    var info = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { ID = id }).ToList();
                    var strId = id.ToString();
                    var strXml = info[0].ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前节点视图内容 (三级储量)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetBaseInfoViewDataById(string id)
        {
            try
            {
                var list = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { ID = new Guid(id), BOC = "三级储量计算单元" });
                var nodeUrl = "/XmlData/ProData/ThreeLevReserve";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var strId = l.ID.ToString();
                    var strXml = l.ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取年份(三级储量，查询)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForYear()
        {
            return Json(_BO_ThreeLevReserveService.GetBaseInfoViewData(new BO_ThreeLevReserveModel()).Where(x => !string.IsNullOrWhiteSpace(x.Year)).OrderBy(x => x.Year), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取储量类别(三级储量，查询)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForCategory(string startYear = "", string endYear = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(startYear))
            {
                model.YearStart = startYear;
            }
            if (!string.IsNullOrWhiteSpace(endYear))
            {
                model.YearEnd = endYear;
            }
            try
            {
                var keys = new List<string>();
                var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).GroupBy(x => x.Category).ToList();
                for (var i = 0; i < list.Count; i++)
                {
                    keys.Add(list[i].Key);
                }
                return Json(keys, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取系(三级储量，查询)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForSeries(string startYear = "", string endYear = "", string category = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(category))
            {
                if (!string.IsNullOrWhiteSpace(startYear))
                {
                    model.YearStart = startYear;
                }
                if (!string.IsNullOrWhiteSpace(endYear))
                {
                    model.YearEnd = endYear;
                }
                model.Category = category;
                try
                {
                    var keys = new List<string>();
                    var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).GroupBy(x => x.Series).ToList();
                    for (var i = 0; i < list.Count; i++)
                    {
                        keys.Add(list[i].Key);
                    }
                    return Json(keys, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex) { throw; }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取组(三级储量，查询)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="category"></param>
        /// <param name="series"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForFormation(string startYear = "", string endYear = "", string category = "", string series = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(series))
            {
                if (!string.IsNullOrWhiteSpace(startYear))
                {
                    model.YearStart = startYear;
                }
                if (!string.IsNullOrWhiteSpace(endYear))
                {
                    model.YearEnd = endYear;
                }
                model.Category = category;
                model.Series = series;
                try
                {
                    var keys = new List<string>();
                    var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).GroupBy(x => x.Formation).ToList();
                    for (var i = 0; i < list.Count; i++)
                    {
                        keys.Add(list[i].Key);
                    }
                    return Json(keys, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex) { }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取层(三级储量，查询)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="category"></param>
        /// <param name="series"></param>
        /// <param name="formation"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForLayer(string startYear = "", string endYear = "", string category = "", string series = "", string formation = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(formation))
            {
                if (!string.IsNullOrWhiteSpace(startYear))
                {
                    model.YearStart = startYear;
                }
                if (!string.IsNullOrWhiteSpace(endYear))
                {
                    model.YearEnd = endYear;
                }
                model.Category = category;
                model.Series = series;
                model.Formation = formation;
                try
                {
                    var keys = new List<string>();
                    var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).GroupBy(x => x.Layer).ToList();
                    for (var i = 0; i < list.Count; i++)
                    {
                        keys.Add(list[i].Key);
                    }
                    return Json(keys, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex) { throw; }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取井区块(三级储量，查询)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="category"></param>
        /// <param name="series"></param>
        /// <param name="formation"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public JsonResult GetBaseInfoViewDataForBlockGuan(string startYear = "", string endYear = "", string category = "", string series = "", string formation = "", string layer = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(layer))
            {
                if (!string.IsNullOrWhiteSpace(startYear))
                {
                    model.YearStart = startYear;
                }
                if (!string.IsNullOrWhiteSpace(endYear))
                {
                    model.YearEnd = endYear;
                }
                model.Category = category;
                model.Series = series;
                model.Formation = formation;
                model.Layer = layer;
                try
                {
                    var keys = new List<string>();
                    var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).GroupBy(x => x.BlockGuan).ToList();
                    for (var i = 0; i < list.Count; i++)
                    {
                        keys.Add(list[i].Key);
                    }
                    return Json(keys, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex) { throw; }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取筛选条件下的视图内容 (三级储量)
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="category"></param>
        /// <param name="series"></param>
        /// <param name="formation"></param>
        /// <param name="layer"></param>
        /// <param name="blockGuan"></param>
        /// <returns></returns>
        public ActionResult GetBaseInfoViewDataByQuery(string startYear = "", string endYear = "", string category = "", string series = "", string formation = "", string layer = "", string blockGuan = "")
        {
            var model = new BO_ThreeLevReserveModel();
            if (!string.IsNullOrWhiteSpace(startYear))
            {
                model.YearStart = startYear;
            }
            if (!string.IsNullOrWhiteSpace(endYear))
            {
                model.YearEnd = endYear;
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                model.Category = category;
            }
            if (!string.IsNullOrWhiteSpace(series))
            {
                model.Series = series;
            }
            if (!string.IsNullOrWhiteSpace(formation))
            {
                model.Formation = formation;
            }
            if (!string.IsNullOrWhiteSpace(layer))
            {
                model.Layer = layer;
            }
            if (!string.IsNullOrWhiteSpace(blockGuan))
            {
                model.BlockGuan = blockGuan;
            }
            try
            {
                var list = _BO_ThreeLevReserveService.GetBaseInfoViewData(model).DistinctBy(x => x.ID).OrderByDescending(x => x.Year).ToList();
                var nodeUrl = "/XmlData/ProData/ThreeLevReserve";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                var flag = true;
                foreach (var l in list)
                {
                    var id = l.ID;
                    var info = _BO_BaseInfoService.GetBO_BaseInfo(new BO_BaseInfoModel { ID = id }).ToList();
                    var strId = id.ToString();
                    var strXml = info[0].ObjectParam;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前节点视图内容(剩余油气/井点分布)
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetWellDisBaseInfoViewData(string name)
        {
            try
            {
                //_BO_WellTestService
                var list = _BO_WellTestService.GetWellDataByXiZuCengNode(name.Trim());
                var nodeUrl = "/XmlData/ProData/WellTest";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                int cycleCount = 0;
                var flag = true;
                var i = 0;
                foreach (var l in list)
                {
                    if (i < 1)
                    {
                        var strId = l.ID.ToString();
                        var strXml = l.ObjectParam;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(strXml);
                        XmlElement root = doc.DocumentElement;
                        XmlNodeList nodeList = root.SelectSingleNode(nodeUrl).ChildNodes;
                        cycleCount += nodeList.Count;
                        if (!string.IsNullOrWhiteSpace(strXml))
                        {
                            XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                        }
                        flag = false;
                    }
                    i++;
                }
                //int realDataCount = 0;
                //if (cycleCount != 0)
                //{
                //    realDataCount = fields.Count() / cycleCount;
                //}
                //fields.RemoveRange(realDataCount, fields.Count() - realDataCount);
                //for (var i = 1; i < fields.Count; i++)
                //{
                //    if (fields[i].Equals("编号"))
                //    {
                //        fields.Remove(fields[i]);
                //    }
                //}
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 拿到BO_WellPoint视图下的所有井点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWellPointGridData()
        {
            var result = _BO_WellPointService.GetWellPointGridData().Select(x => new BO_WellPointModel { ID = x.ID, Name = x.Name, BlockGuan = x.BlockGuan, BlockType = x.BlockType });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 拿到BO_WellTest视图下所有的试油成果数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWellTestResultGridData()
        {
            try
            {
                var list = _BO_WellTestService.GetWellTestResultGridData();
                var nodeUrl = "/XmlData/ProData/WellTest";
                var fields = new List<string>();
                var fieldModel = new List<string>();
                var headTitles = new List<Dictionary<string, string>>();
                var records = new List<Dictionary<string, string>>();
                int cycleCount = 0;
                var flag = true;
                foreach (var l in list)
                {
                    var strId = l.ID.ToString();
                    var strXml = l.ObjectParam;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXml);
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList nodeList = root.SelectSingleNode(nodeUrl).ChildNodes;
                    cycleCount += nodeList.Count;
                    if (!string.IsNullOrWhiteSpace(strXml))
                    {
                        XmlParse(ref fields, ref fieldModel, ref headTitles, ref records, nodeUrl, strXml, strId, flag);
                    }
                    flag = false;
                }
                //int realDataCount = 0;
                //if (cycleCount != 0)
                //{
                //    realDataCount = fields.Count() / cycleCount;
                //}
                //fields.RemoveRange(realDataCount, fields.Count() - realDataCount);
                //for (var i = 1; i < fields.Count; i++)
                //{
                //    if (fields[i].Equals("编号"))
                //    {
                //        fields.Remove(fields[i]);
                //    }
                //}
                StringBuilder _fields = new StringBuilder();
                StringBuilder _fieldModel = new StringBuilder();
                StringBuilder _headTitles = new StringBuilder();
                StringBuilder _records = new StringBuilder();
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                jss.Serialize(fields, _fields);
                jss.Serialize(fieldModel, _fieldModel);
                jss.Serialize(headTitles, _headTitles);
                jss.Serialize(records, _records);

                var str = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
                return Content(str);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 通过树节点SID拿到对应的井点数据
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public ActionResult GetWellPointGridDataBySID(string sid)
        {
            var result = _BO_WellPointService.GetWellPointGridDataBySID(sid).Select(x => new BO_WellPointModel { ID = x.ID, Name = x.Name, BlockGuan = x.BlockGuan, BlockType = x.BlockType });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchWellPointData(string sid, string blockGuan, string blockType, string blockName)
        {
            var result = _BO_WellPointService.SearchWellPointData(sid, blockGuan, blockType, blockName).Select(x => new BO_WellPointModel { ID = x.ID, Name = x.Name, BlockGuan = x.BlockGuan, BlockType = x.BlockType, Elevation = x.Elevation, XCoordinate = x.XCoordinate, YCoordinate = x.YCoordinate }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 拿到井区下拉框的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBlockGuanData(string sid)
        {
            //var result = _BO_WellPointService.GetAllData().Select(x => new BlockGuanModel { blockGuan = x.BlockGuan }).Distinct();
            var result = _BO_WellPointService.GetWellPointGridDataBySID(sid).Select(x => new BlockGuanModel { blockGuan = x.BlockGuan }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 拿到井别下拉框的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBlockTypeData(string sid)
        {
            //var result = _BO_WellPointService.GetAllData().Select(x => new BlockTypeModel { blockType = x.BlockType }).Distinct();
            var result = _BO_WellPointService.GetWellPointGridDataBySID(sid).Select(x => new BlockTypeModel { blockType = x.BlockType }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    public class BlockGuanModel
    {
        public string blockGuan { get; set; }
    }

    public class BlockTypeModel
    {
        public string blockType { get; set; }
    }
}