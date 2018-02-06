using Jurassic.AppCenter;
using Jurassic.WebFrame;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class GeoSearchController : BaseController
    {
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"]
            + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];
        private readonly string _achieveDown = System.Configuration.ConfigurationManager.AppSettings["AchieveDown"];

        /// Index视图
        public ActionResult Index()
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
        /// Table视图
        public ActionResult Table()
        {
            return View();
        }
        public ActionResult TableXml()
        {
            var Data = new List<Dictionary<string, string>>();
            var Names = new List<string>();
            var NamesModel = new List<string>();
            var Head = new List<Dictionary<string, string>>();


            var url = System.Web.HttpContext.Current.Server.MapPath("/") + ("/Local/test/xml/ReserveOil.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url); 

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("ProData").ChildNodes;

            foreach (XmlNode xn in nodeList)
            {
                var attr = xn.Attributes;
                for (var i = 0; i < attr.Count; i++)
                {
                    var name = attr[i].Name; var val = attr[i].Value;
                    if (name == "Class" && val == "Oil")
                    {
                        var _data = new Dictionary<string, string>();
                        for (var j = 0; j < xn.ChildNodes.Count; j++)
                        {

                            if (xn.ChildNodes[j].FirstChild.ChildNodes.Count > 0)
                            {
                                var head = new Dictionary<string, string>();
                                var titleText = "";
                                var count = xn.ChildNodes[j].Attributes.Count;//{ "numberOfColumns": 3, "titleText": "General Info", "startColumnName": "ID" },
                                for (var jj = 0; jj < count; jj++)
                                {
                                    if (xn.ChildNodes[j].Attributes[jj].Name == "caption")
                                    {
                                        titleText = xn.ChildNodes[j].Attributes[jj].Value;
                                    }
                                }
                                var numberOfColumns = xn.ChildNodes[j].ChildNodes.Count.ToString();
                                var startColumnName = xn.ChildNodes[j].FirstChild.Name;
                                head.Add("numberOfColumns", numberOfColumns);
                                head.Add("titleText", titleText);
                                head.Add("startColumnName", startColumnName);
                                Head.Add(head);

                                var cnodeList = xn.ChildNodes[j].ChildNodes;
                                foreach (XmlNode cxn in cnodeList)
                                {
                                    if (cxn.FirstChild.ChildNodes.Count > 0)
                                    {
                                        var head2 = new Dictionary<string, string>();
                                        var titleText2 = "";
                                        var count2 = cxn.ChildNodes[j].Attributes.Count;
                                        for (var jj = 0; jj < count; jj++)
                                        {
                                            if (cxn.ChildNodes[j].Attributes[jj].Name == "caption")
                                            {
                                                titleText = cxn.ChildNodes[j].Attributes[jj].Value;
                                            }
                                        }
                                        var numberOfColumns2 = cxn.ChildNodes[j].ChildNodes.Count.ToString();
                                        var startColumnName2 = cxn.ChildNodes[j].FirstChild.Name;
                                        head.Add("numberOfColumns", numberOfColumns2);
                                        head.Add("titleText", titleText2);
                                        head.Add("startColumnName", startColumnName2);
                                        Head.Add(head2);
                                    }
                                    else
                                    {
                                        var key = cxn.Name;
                                        var value = cxn.InnerText;
                                        NamesModel.Add(key);
                                        _data.Add(key, value);

                                        var len = cxn.Attributes.Count;
                                        var str = "";
                                        for (var ii = 0; ii < len; ii++)
                                        {
                                            if (cxn.Attributes[ii].Name == "caption")
                                            {
                                                str += cxn.Attributes[ii].Value;
                                            }
                                            if (cxn.Attributes[ii].Name == "units")
                                            {
                                                str += "<br/>" + cxn.Attributes[ii].Value;
                                            }
                                        }
                                        Names.Add(str);
                                    }
                                }
                            }
                            else
                            {
                                var key = xn.FirstChild.Name;
                                var value = xn.FirstChild.InnerText;
                                _data.Add(key, value);
                                NamesModel.Add(key);
                                var len = xn.FirstChild.Attributes.Count;
                                var str = "";
                                for (var ii = 0; ii < len; ii++)
                                {
                                    if (xn.FirstChild.Attributes[ii].Name == "caption")
                                    {
                                        str += xn.FirstChild.Attributes[ii].Value;
                                    }
                                    if (xn.FirstChild.Attributes[ii].Name == "units")
                                    {
                                        str += "<br/>" + xn.FirstChild.Attributes[ii].Value;
                                    }
                                }
                                Names.Add(str);
                            }
                            Data.Add(_data);
                        }
                    }
                }
            }
            var d3 = Data.Distinct().ToList();
            var d1 = Names.Distinct().ToList();
            var d0 = NamesModel.Distinct().ToList();
            var d2 = Head.Distinct().ToList();
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            StringBuilder sb4 = new StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            jss.Serialize(d0, sb1);
            jss.Serialize(d1, sb2);
            jss.Serialize(d2, sb3);
            jss.Serialize(d3, sb4);
            var d = sb1 + "#" + sb2 + "#" + sb3 + "#" + sb4 + "#";

            return Content(d);
        }

        public ActionResult TableXml2()
        {
            var fields = new List<string>();
            var fieldModel = new List<string>();
            var headTitles = new List<Dictionary<string, string>>();
            var records = new List<Dictionary<string, string>>();

            var strId = "00000001";
            var url = System.Web.HttpContext.Current.Server.MapPath("/") + ("/Local/test/xml/grouptitle.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("/XmlData/ProData/WellTest").ChildNodes;
            if (nodeList != null)
            {
                var flag = true;
                foreach (XmlNode xn in nodeList)
                {
                    var record = new Dictionary<string, string>();
                    for (var i = 0; i < xn.ChildNodes.Count; i++)
                    {
                        var currentNode = xn.ChildNodes[i];
                        if (currentNode.FirstChild != null && currentNode.FirstChild.NodeType == System.Xml.XmlNodeType.Element)
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
                            if (flag)
                            {
                                headTitle1.Add("numberOfColumns", numberOfColumns);
                                headTitle1.Add("titleText", titleText);
                                headTitle1.Add("startColumnName", startColumnName);
                                headTitles.Add(headTitle1);
                            }

                            var cNodeList = currentNode.ChildNodes;
                            foreach (XmlNode cxn in cNodeList)
                            {
                                //for (var ii = 0; ii < cxn.ChildNodes.Count; ii++)
                                // {
                                var currentNode2 = cxn;
                                //if (currentNode2.FirstChild != null && currentNode2.FirstChild.NodeType != System.Xml.XmlNodeType.Element) 
                                //{
                                //    var headTitle2 = new Dictionary<string, string>();
                                //    var titleText2 = "";
                                //    for (var kk = 0; currentNode2.Attributes != null && kk < currentNode2.Attributes.Count; kk++)
                                //    {
                                //        if (currentNode2.Attributes[kk].Name == "caption")
                                //        {
                                //            titleText2 = currentNode2.Attributes[kk].Value;
                                //        }
                                //    }
                                //    var numberOfColumns2 = currentNode2.ChildNodes.Count.ToString();
                                //    var startColumnName2 = currentNode2.FirstChild.Name;
                                //    headTitle2.Add("numberOfColumns", numberOfColumns2);
                                //    headTitle2.Add("titleText", titleText2);
                                //    headTitle2.Add("startColumnName", startColumnName2);
                                //    headTitles.Add(headTitle2);
                                //}
                                //else
                                //{
                                var isShow = true;
                                var str = "";
                                for (var m = 0; currentNode2.Attributes != null && m < currentNode2.Attributes.Count; m++)
                                {
                                    if (currentNode2.Attributes[m].Name == "caption")
                                    {
                                        str += currentNode2.Attributes[m].Value;
                                    }
                                    if (currentNode2.Attributes[m].Name == "units")
                                    {
                                        str += "<br/>" + currentNode2.Attributes[m].Value;
                                    }
                                    if (currentNode2.Attributes[m].Name == "display" && currentNode2.Attributes[m].Value == "fasle")
                                    {
                                        isShow = false;
                                    }
                                }
                                if (isShow)
                                {
                                    var key = currentNode2.Name;
                                    var value = currentNode2.InnerText.Trim();
                                    if (flag)
                                    {
                                        if (!fields.Contains("编号"))
                                        {
                                            fields.Add("编号");
                                        }
                                        fields.Add(str);
                                        if (!fieldModel.Contains("ID"))
                                        {
                                            fieldModel.Add("ID");
                                        }
                                        fieldModel.Add(key);
                                    }
                                    if (!record.ContainsKey("ID"))
                                    {
                                        record.Add("ID", strId);
                                    }
                                    record.Add(key, value);
                                }
                                //}
                                // }
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
                                if (currentNode.Attributes[m].Name == "display" && currentNode.Attributes[m].Value == "false")
                                {
                                    isShow = false;
                                }
                            }
                            if (isShow)
                            {
                                var key = currentNode.Name;
                                var value = currentNode.InnerText.Trim();
                                if (flag)
                                {
                                    if (!fields.Contains("编号"))
                                    {
                                        fields.Add("编号");
                                    }
                                    fields.Add(str);
                                    if (!fieldModel.Contains("ID"))
                                    {
                                        fieldModel.Add("ID");
                                    }
                                    fieldModel.Add(key);
                                }
                                if (!record.ContainsKey("ID"))
                                {
                                    record.Add("ID", strId);
                                }
                                record.Add(key, value);
                            }
                        }
                    }
                    records.Add(record);
                    flag = false;
                }
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

            var result = _fields + "#" + _fieldModel + "#" + _headTitles + "#" + _records;
            return Content(result);
        }
    }
}