using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;
using NPOI.HSSF.UserModel;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace Jurassic.RECP.DataService.Service
{
    public class CommonService : ICommonService
    {
        /// <summary>
        /// 数据层接口
        /// </summary> 
        private IBO_BaseInfoEFRepository BO_BaseInfoEFRepository { get; set; }
        private IBO_ProMetaDefineEFRepository BO_ProMetaDefineEFRepository { get; set; }
        private IBO_ProMetaAttributeEFRepository BO_ProMetaAttributeEFRepository { get; set; }

        private ITemp_ImportProDataEFRepository temp_ImportProDataEFRepository { get; set; }
        private IV_Temp_ImportProDataEFRepository v_Temp_ImportProDataEFRepository { get; set; }
        private IV_Temp_ImportBOEFRepository v_Temp_ImportBOEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="_IBO_BaseInfoEFRepository"></param>
        /// <param name="_IBO_ProMetaDefineEFRepository"></param>
        /// <param name="_IBO_ProMetaAttributeEFRepository"></param>
        /// <param name="_ITemp_ImportProDataEFRepository"></param>
        /// <param name="_IV_Temp_ImportProDataEFRepository"></param>
        /// <param name="_IV_Temp_ImportBOEFRepository"></param>
        public CommonService(IBO_BaseInfoEFRepository _IBO_BaseInfoEFRepository, IBO_ProMetaDefineEFRepository _IBO_ProMetaDefineEFRepository, IBO_ProMetaAttributeEFRepository _IBO_ProMetaAttributeEFRepository, ITemp_ImportProDataEFRepository _ITemp_ImportProDataEFRepository, IV_Temp_ImportProDataEFRepository _IV_Temp_ImportProDataEFRepository, IV_Temp_ImportBOEFRepository _IV_Temp_ImportBOEFRepository)
        {
            BO_BaseInfoEFRepository = _IBO_BaseInfoEFRepository;
            BO_ProMetaDefineEFRepository = _IBO_ProMetaDefineEFRepository;
            BO_ProMetaAttributeEFRepository = _IBO_ProMetaAttributeEFRepository;
            temp_ImportProDataEFRepository = _ITemp_ImportProDataEFRepository;
            v_Temp_ImportProDataEFRepository = _IV_Temp_ImportProDataEFRepository;
            v_Temp_ImportBOEFRepository = _IV_Temp_ImportBOEFRepository;
        }

        //保存样式导出单元格样式
        private ICellStyle cellStyle = null;

        /// <summary>
        /// 获取专业元数据定义列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<RECP_BO_ProMetaDefineModel> GetBO_ProMetaDefine(RECP_BO_ProMetaDefineModel model)
        {
            IEnumerable<RECP_BO_ProMetaDefine> info = BO_ProMetaDefineEFRepository.GetQuery();
            //if (model.ID.ToString() != Guid.Empty.ToString())
            //{
            //    info = info.Where(x => x.ID == model.ID);
            //} 
            return info.Select(Mapper.Map<RECP_BO_ProMetaDefine, RECP_BO_ProMetaDefineModel>).ToList();
        }

        /// <summary>
        /// 获取专业元数据属性列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<RECP_BO_ProMetaAttributeModel> GetBO_ProMetaAttribute(RECP_BO_ProMetaAttributeModel model)
        {
            IEnumerable<RECP_BO_ProMetaAttribute> info = BO_ProMetaAttributeEFRepository.GetQuery();
            //if (model.ID.ToString() != Guid.Empty.ToString())
            //{
            //    info = info.Where(x => x.ID == model.ID);
            //} 
            return info.Select(Mapper.Map<RECP_BO_ProMetaAttribute, RECP_BO_ProMetaAttributeModel>).ToList();
        }

        /// <summary>
        /// 创建Excel 接口方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="metaEntity"></param>
        public void CreateExcel(string fileName, string metaEntity)
        {
            var filePath = FileDirectoryOperate(fileName);
            CreateExcelWithData(fileName, filePath, metaEntity);
        }

        /// <summary>
        /// 把文件流数据导到workbook
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="metaEntity"></param>
        private void CreateExcelWithData(string fileName, string filePath, string metaEntity)
        {
            IWorkbook wb = null;
            var sheetName = fileName + "专业数据";
            using (FileStream fs = File.OpenRead(filePath))
            {
                wb = SelectWorkBook(fs, filePath);
                var sheet = wb.CreateSheet(sheetName);
                DefineCellStyle(wb);
                DataToWorkBook(sheet, metaEntity);
            }
            WorkBookToExcelFile(wb, filePath);
        }

        /// <summary>
        /// 通过data 创建Excel workbook实体
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="metaEntity"></param>
        private void DataToWorkBook(ISheet sheet, string metaEntity)
        {
            var metaDefineInfo = BO_ProMetaDefineEFRepository.GetQuery().Where(x => x.MetaEntity == metaEntity);
            var info = metaDefineInfo.Select(Mapper.Map<RECP_BO_ProMetaDefine, RECP_BO_ProMetaDefineModel>)
                .OrderBy(x => x.FieldIndex).Where(x => x.FieldIndex != -1 && x.FieldIndex != 0).ToList();

            var multiLevel = info.Where(x => x.ParentNo != info[0].ParentNo).Count() > 0;
            if (!multiLevel)
            {
                var row0 = sheet.CreateRow(0);
                for (var n = 0; n < info.Count; n++)
                {
                    var model = info[n];
                    if (model != null)
                    {
                        var fieldNo = model.FieldNo;
                        var fieldText = GetFieldText(fieldNo);
                        SetCell(row0.CreateCell(n), fieldText);
                    }
                }
            }
            else
            {
                for (var i = 0; i < 2; i++)
                {
                    var row = sheet.CreateRow(i);
                    var k = 0; // 显示列数，从第0列开始
                    for (var j = 0; j < info.Count; j++)
                    {
                        var model = info[j];
                        if (model != null)
                        {
                            var fieldNo = model.FieldNo;
                            var parentNo = model.ParentNo;
                            var fieldText = GetFieldText(fieldNo);

                            var hasParent = HasParent(parentNo, info);
                            var isParent = IsParentNode(fieldNo, info);
                            int startRowIndex = 0, endRowIndex = 0, startColIndex = 0, endColIndex = 0;
                            if (i == 0)
                            {
                                int count = ChildrenCount(fieldNo, info);
                                if (!hasParent && count == 0)
                                {
                                    SetCell(row.CreateCell(k++), fieldText);
                                    startRowIndex = 0;
                                    endRowIndex = 1;
                                    startColIndex = endColIndex = k - 1; // 行合并
                                }
                                if (count > 1)
                                {
                                    startRowIndex = 0;
                                    endRowIndex = 0;
                                    startColIndex = k;
                                    endColIndex = count + k - 1; // 列合并
                                }
                                for (var m = 0; isParent && m < count; m++)
                                {
                                    SetCell(row.CreateCell(k++), fieldText);
                                }
                                sheet.AddMergedRegion(new CellRangeAddress(startRowIndex, endRowIndex, startColIndex,
                                    endColIndex));
                            }
                            if (i == 1)
                            {
                                if (!isParent)
                                {
                                    SetCell(row.CreateCell(k++), fieldText);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 计算当前节点子节点数量
        /// </summary>
        /// <param name="fieldNo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private int ChildrenCount(Guid fieldNo, List<RECP_BO_ProMetaDefineModel> info)
        {
            return info.Where(x => x.ParentNo == fieldNo).Count();
        }

        /// <summary>
        /// 获取字段属性 信息
        /// </summary>
        /// <param name="fieldNo"></param>
        /// <returns></returns>
        private string GetFieldText(Guid fieldNo)
        {
            var str = string.Empty;
            var metaAttributeInfo = BO_ProMetaAttributeEFRepository.GetQuery();
            if (fieldNo != Guid.Empty)
            {
                metaAttributeInfo = metaAttributeInfo.Where(x => x.FieldNo == fieldNo);
            }
            var info =
                metaAttributeInfo.Select(Mapper.Map<RECP_BO_ProMetaAttribute, RECP_BO_ProMetaAttributeModel>).ToList();
            foreach (var li in info)
            {
                var code = li.AttrCode;
                if (code.Trim() == "caption")
                {
                    str += li.AttrValue;
                }
                if (code.Trim() == "units" && !string.IsNullOrWhiteSpace(li.AttrValue))
                {
                    str += "NewLine" + li.AttrValue;
                }
            }
            return str;
        }

        /// <summary>
        /// 判断当前节点是否为父节点
        /// </summary>
        /// <param name="fieldNo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool IsParentNode(Guid fieldNo, List<RECP_BO_ProMetaDefineModel> info)
        {
            return info.Find(x => x.ParentNo == fieldNo) != null;
        }

        /// <summary>
        /// 判断当前节点是否有父节点
        /// </summary>
        /// <param name="parentNo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool HasParent(Guid? parentNo, List<RECP_BO_ProMetaDefineModel> info)
        {
            return info.Find(x => x.FieldNo == parentNo) != null;
        }

        /// <summary>
        /// 定义单元格样式
        /// </summary>
        /// <param name="wk"></param>
        private void DefineCellStyle(IWorkbook wk)
        {
            ICellStyle style = wk.CreateCellStyle();
            IFont font = wk.CreateFont();
            font.FontName = "微软雅黑";
            style.SetFont(font);

            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            cellStyle = style;
        }

        /// <summary>
        /// 根据WorkBook 生成excel
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="filePath"></param>
        private void WorkBookToExcelFile(IWorkbook xls, string filePath)
        {
            FileStream fs = File.Create(filePath);
            xls.Write(fs);
            fs.Close();
        }

        private string FileDirectoryOperate(string fileName)
        {
            var configPath = ConfigurationManager.AppSettings["FileFolderPath"] != null
                ? ConfigurationManager.AppSettings["FileFolderPath"].ToString()
                : "";
            var fileFolderPath = HttpContext.Current.Request.PhysicalApplicationPath + configPath;
            var filePath = HttpContext.Current.Request.PhysicalApplicationPath + configPath + fileName + ".xls";
            if (!Directory.Exists(fileFolderPath))
            {
                Directory.CreateDirectory(fileFolderPath);
            }
            CreateFile(filePath);
            return filePath;
        }

        private void CreateFile(string filePath)
        {
            var wb = SelectWorkBook(null, filePath);
            var fs = new FileStream(filePath, FileMode.Create);
            wb.Write(fs);
            fs.Close();
        }

        private IWorkbook SelectWorkBook(FileStream fs, string filePath)
        {
            IWorkbook wb = null;
            if (filePath.IndexOf(".xlsx") > 0)
            {
                wb = fs != null ? new XSSFWorkbook(fs) : new XSSFWorkbook();
            }
            if (filePath.IndexOf(".xls") > 0)
            {
                wb = fs != null ? new HSSFWorkbook(fs) : new HSSFWorkbook();
            }
            return wb;
        }

        /// <summary>
        /// 单元格（赋值 & 样式）
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellValue">值</param>
        private void SetCell(ICell cell, string cellValue)
        {
            cell.SetCellValue(cellValue.Replace("NewLine", Environment.NewLine));
            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// 把excel文件数据保存到表中（临时表，数据更新完，会进行删除操作）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaEntity"></param>
        /// <param name="start"></param>
        /// <param name="operSN"></param>
        public void SaveExcelToTemp(string filePath, string metaEntity, int start, Guid operSN)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                var wk = SelectWorkBook(fs, filePath);
                var sheet = wk.GetSheetAt(0);
                for (int j = start - 1; j <= sheet.LastRowNum; j++) //LastRowNum 是当前表的总行数
                {
                    IRow row = sheet.GetRow(j); //读取当前行数据   读取表格数据
                    if (row != null)
                    {
                        var emptyRow = true;
                        var model = new Temp_ImportProDataModel();
                        RowInfoMappingToEntity(model, row, metaEntity, operSN, ref emptyRow);
                        Temp_ImportProData _model = Mapper.Map<Temp_ImportProDataModel, Temp_ImportProData>(model);
                        if (!emptyRow)
                        {
                            temp_ImportProDataEFRepository.Add(_model);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 行数据映射到实体model ，A 字段默认为主键，校验
        /// </summary>
        /// <param name="model"></param>
        /// <param name="row"></param>
        /// <param name="metaEntity"></param>
        /// <param name="guid"></param>
        /// <param name="emptyRow"></param>
        private void RowInfoMappingToEntity(Temp_ImportProDataModel model, IRow row, string metaEntity, Guid guid, ref bool emptyRow)
        {
            model.ID = Guid.NewGuid();
            model.MetaEntity = metaEntity;
            model.OperSN = guid;
            model.ExecState = 0;
            if (row.GetCell(0) != null)
            {
                model.A = row.GetCell(0).ToString();
                emptyRow = false;
            }
            if (row.GetCell(1) != null) model.B = row.GetCell(1).ToString();
            if (row.GetCell(2) != null) model.C = row.GetCell(2).ToString();
            if (row.GetCell(3) != null) model.D = row.GetCell(3).ToString();
            if (row.GetCell(4) != null) model.E = row.GetCell(4).ToString();
            if (row.GetCell(5) != null) model.F = row.GetCell(5).ToString();
            if (row.GetCell(6) != null) model.G = row.GetCell(6).ToString();
            if (row.GetCell(7) != null) model.H = row.GetCell(7).ToString();
            if (row.GetCell(8) != null) model.I = row.GetCell(8).ToString();
            if (row.GetCell(9) != null) model.J = row.GetCell(9).ToString();
            if (row.GetCell(10) != null) model.K = row.GetCell(10).ToString();
            if (row.GetCell(11) != null) model.L = row.GetCell(11).ToString();
            if (row.GetCell(12) != null) model.M = row.GetCell(12).ToString();
            if (row.GetCell(13) != null) model.N = row.GetCell(13).ToString();
            if (row.GetCell(14) != null) model.O = row.GetCell(14).ToString();
            if (row.GetCell(15) != null) model.P = row.GetCell(15).ToString();
            if (row.GetCell(16) != null) model.Q = row.GetCell(16).ToString();
            if (row.GetCell(17) != null) model.R = row.GetCell(17).ToString();
            if (row.GetCell(18) != null) model.S = row.GetCell(18).ToString();
            if (row.GetCell(19) != null) model.T = row.GetCell(19).ToString();
            if (row.GetCell(20) != null) model.U = row.GetCell(20).ToString();
            if (row.GetCell(21) != null) model.V = row.GetCell(21).ToString();
            if (row.GetCell(22) != null) model.W = row.GetCell(22).ToString();
            if (row.GetCell(23) != null) model.X = row.GetCell(23).ToString();
            if (row.GetCell(24) != null) model.Y = row.GetCell(24).ToString();
            if (row.GetCell(25) != null) model.Z = row.GetCell(25).ToString();
            if (row.GetCell(26) != null) model.AA = row.GetCell(26).ToString();
            if (row.GetCell(27) != null) model.AB = row.GetCell(27).ToString();
            if (row.GetCell(28) != null) model.AC = row.GetCell(28).ToString();
            if (row.GetCell(29) != null) model.AD = row.GetCell(29).ToString();
            if (row.GetCell(30) != null) model.AE = row.GetCell(30).ToString();
            if (row.GetCell(31) != null) model.AF = row.GetCell(31).ToString();
            if (row.GetCell(32) != null) model.AG = row.GetCell(32).ToString();
            if (row.GetCell(33) != null) model.AH = row.GetCell(33).ToString();
            if (row.GetCell(34) != null) model.AI = row.GetCell(34).ToString();
            if (row.GetCell(35) != null) model.AJ = row.GetCell(35).ToString();
            if (row.GetCell(36) != null) model.AK = row.GetCell(36).ToString();
            if (row.GetCell(37) != null) model.AL = row.GetCell(37).ToString();
            if (row.GetCell(38) != null) model.AM = row.GetCell(38).ToString();
            if (row.GetCell(39) != null) model.AN = row.GetCell(39).ToString();
            if (row.GetCell(40) != null) model.AO = row.GetCell(40).ToString();
        }

        /// <summary>
        /// 获取临时表对应的视图中的数据，用于页面显示
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<V_Temp_ImportProDataModel> GetV_TempImportProData(V_Temp_ImportProDataModel model)
        {
            IEnumerable<V_Temp_ImportProData> info = v_Temp_ImportProDataEFRepository.GetQuery();
            if (!string.IsNullOrWhiteSpace(model.MetaEntity))
            {
                info = info.Where(x => x.MetaEntity == model.MetaEntity);
            }
            if (model.OperSN != Guid.Empty)
            {
                info = info.Where(x => x.OperSN == model.OperSN);
            }
            return info.Select(Mapper.Map<V_Temp_ImportProData, V_Temp_ImportProDataModel>).OrderBy(x => x.A).ToList();
        }

        /// <summary>
        /// 根据操作流水号，删除相应临时表数据
        /// </summary>
        /// <param name="opSN"></param>
        public void DeleteTempProData(Guid opSN)
        {
            temp_ImportProDataEFRepository.DeleteList(x => x.OperSN == opSN);
        }

        /// <summary>
        /// 临时表中的数据保存到baseinfo 表中
        /// </summary>
        /// <param name="operSN"></param>
        /// <param name="metaEntity"></param>
        /// <param name="userName"></param>
        /// <param name="rId"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        public void SaveExcelTmpToBO(Guid operSN, string metaEntity, string userName, Guid rId, string bot, string boc)
        {
            //获取参照数据源    
            var bInfo = v_Temp_ImportBOEFRepository.GetQuery().Where(x => x.OperSN == operSN && x.MetaEntity == metaEntity).ToList();
            //循环获取name 
            foreach (var info in bInfo)
            {
                var name = info.Name;
                var list = temp_ImportProDataEFRepository.GetQuery().Where(x => x.OperSN == operSN && x.A == name).ToList(); //通过name获取temp筛选数据
                var metaDefine = BO_ProMetaDefineEFRepository.GetQuery().Where(x => x.MetaEntity == metaEntity).Select(Mapper.Map<RECP_BO_ProMetaDefine, RECP_BO_ProMetaDefineModel>);
                var strXml = "";
                var strNode = "";
                foreach (var model in list) //records count
                {
                    strXml += GetStrXmlBody(model, metaDefine);
                }
                var strProDataXml = GetProDataXml(metaDefine, strXml, ref strNode);
                var newStrXml = "";
                var strNodeUrl = "/XmlData/ProData" + "/" + strNode;

                #region xml操作 
                var strXmlData = "<XmlData></XmlData>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXmlData);
                XmlElement root = doc.DocumentElement;
                var hasFeatureNode = root.SelectSingleNode("/XmlData/Feature") != null;
                if (!hasFeatureNode)
                {
                    var strFeatureXml = GetFeatureXml();
                    var xRoot = XElement.Parse(strXmlData);
                    var xFeature = XElement.Parse(strFeatureXml);
                    xRoot.Add(xFeature);
                    newStrXml += xRoot.ToString();
                }
                if (info.BOExist == 1)
                {
                    #region  更新
                    var bo = BO_BaseInfoEFRepository.Find(x => x.Name == name);
                    if (bo != null)
                    {
                        var strOriXml = bo.ObjectParam;
                        XmlDocument updateXmlD = new XmlDocument();
                        updateXmlD.LoadXml(strOriXml);
                        XmlElement updateXmlE = updateXmlD.DocumentElement;
                        var hasProDataXNode = updateXmlE.SelectSingleNode(strNodeUrl) != null;

                        var xRoot = XElement.Parse(strOriXml);
                        var xProDataXNode = XElement.Parse(strProDataXml);
                        if (!hasProDataXNode)
                        {
                            if (xRoot.Elements().Where(x => x.Name.LocalName == "ProData").First() != null)
                                xRoot.Elements().Where(x => x.Name.LocalName == "ProData").First().Add(xProDataXNode.Elements().First());
                        }
                        else
                        {
                            xRoot.Elements().Where(x => x.Name.LocalName == "ProData").Elements().Where(x => x.Name.LocalName == strNode).Remove();
                            xRoot.Elements().Where(x => x.Name.LocalName == "ProData").First().Add(xProDataXNode.Elements().First());
                        }
                        bo.ObjectParam = xRoot.ToString();
                        bo.LastUpdatedBy = userName;
                        bo.LastUpdatedDate = DateTime.Now;
                        BO_BaseInfoEFRepository.Update(bo);
                    }
                    #endregion
                }
                else
                {
                    #region  新增 
                    XmlDocument addXmlD = new XmlDocument();
                    addXmlD.LoadXml(newStrXml);
                    XmlElement addXmlE = addXmlD.DocumentElement;
                    var hasProDataXNode = addXmlE.SelectSingleNode(strNodeUrl) != null;

                    var xRoot = XElement.Parse(newStrXml);
                    var xProDataXNode = XElement.Parse(strProDataXml);
                    if (!hasProDataXNode)
                    {
                        xRoot.Add(xProDataXNode);
                    }
                    BO_BaseInfoEFRepository.Add(new BO_BaseInfo() { ID = Guid.NewGuid(), PID = rId, Name = name, ObjectParam = xRoot.ToString(), BOT = bot, BOC = boc, CreatedBy = userName, CreatedDate = DateTime.Now });
                    #endregion
                }
                UpdateTemp_ImportProDataState(name, operSN, metaEntity);
                #endregion
            }
        }

        /// <summary>
        /// 更新临时表中的数据状态，用于页面显示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="operSN"></param>
        /// <param name="metaEntity"></param>
        private void UpdateTemp_ImportProDataState(string name, Guid operSN, string metaEntity)
        {
            var infos = temp_ImportProDataEFRepository.GetQuery()
                    .Where(x => x.OperSN == operSN && x.A == name && x.MetaEntity == metaEntity).ToList();
            foreach (var info in infos)
            {
                info.ExecState = 1;
                temp_ImportProDataEFRepository.Update(info);
            }
        }

        /// <summary>
        /// 获取专业xml数据 迭代
        /// </summary>
        /// <param name="metaDefine"></param>
        /// <param name="strXml"></param>
        /// <param name="strNode"></param>
        /// <returns></returns>
        private string GetProDataXml(IEnumerable<RECP_BO_ProMetaDefineModel> metaDefine, string strXml, ref string strNode)
        {
            var result = "";
            var tag = metaDefine.Where(x => x.ParentNo == null).ToList();
            if (tag.Count > 0)
            {
                var id = tag[0].FieldNo;
                var fieldCode = tag[0].FieldCode;
                var info = metaDefine.Where(x => x.FieldIndex == -1 && x.ParentNo == id).ToList();
                strNode = info.Count > 0 ? info[0].FieldCode : "";
                result += "<" + fieldCode + " " + GetNodeAttrInfo(id) + ">";
                result += GetHeadChildren(id, info, strXml);
                result += "</" + fieldCode + ">";
            }
            return result;
        }

        /// <summary>
        /// 获取特征属性xml字符串
        /// </summary>
        /// <returns></returns>
        private string GetFeatureXml()
        {
            var result = "";
            result += "<Feature>";
            result += "<Reserve>";
            result += "<GeoMap url=\"\">";
            result += "<Layer name=\"\">";
            result += "<Element name=\"\" id=\"\">";
            result += "</Element>";
            result += "</Layer>";
            result += "</GeoMap>";
            result += "</Reserve>";
            result += "<Well>";
            result += "<GeoMap url=\"\">";
            result += "<Layer name=\"\">";
            result += "<Element name=\"\" id=\"\">";
            result += "</Element>";
            result += "</Layer>";
            result += "</GeoMap>";
            result += "</Well>";
            result += "</Feature>";
            return result;
        }

        private string GetHeadChildren(Guid id, List<RECP_BO_ProMetaDefineModel> baseInfo, string strXml)
        {
            var result = "";
            var infos = baseInfo.Where(x => x.ParentNo == id).ToList();
            if (infos.Count > 0)
            {
                foreach (var info in infos)
                {
                    var fieldNo = info.FieldNo;
                    var fieldCode = info.FieldCode;
                    result += "<" + fieldCode + " " + GetNodeAttrInfo(id) + ">";
                    result += GetHeadChildren(fieldNo, baseInfo, strXml);
                    result += "</" + fieldCode + ">";
                }
            }
            else
            {
                result = strXml;
            }
            return result;
        }

        /// <summary>
        /// 获取节点属性信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private string GetNodeAttrInfo(Guid Id)
        {
            var str = string.Empty;
            var baseInfo = BO_ProMetaAttributeEFRepository.GetQuery();
            if (Id != Guid.Empty)
            {
                baseInfo = baseInfo.Where(x => x.FieldNo == Id);
            }
            var infos = baseInfo.Select(Mapper.Map<RECP_BO_ProMetaAttribute, RECP_BO_ProMetaAttributeModel>).ToList();
            foreach (var info in infos)
            {
                str += " " + info.AttrCode + "=\"" + info.AttrValue + "\"";
            }
            return str;
        }

        /// <summary>
        /// 生成迭代体的xml字符串
        /// </summary>
        /// <param name="model"></param>
        /// <param name="metaDefine"></param>
        /// <returns></returns>
        private string GetStrXmlBody(Temp_ImportProData model, IEnumerable<RECP_BO_ProMetaDefineModel> metaDefine)
        {
            //转xml字符串
            var result = "";
            var tag = metaDefine.Where(x => x.FieldIndex == 0).ToList();
            var tagFieldCode = tag.Count > 0 ? tag[0].FieldCode : "";
            if (!string.IsNullOrWhiteSpace(tagFieldCode))
            {
                var tagFieldNo = tag[0].FieldNo;
                result += "<" + tagFieldCode + " " + GetNodeAttrInfo(tagFieldNo) + ">";
                var info = metaDefine.OrderBy(x => x.FieldIndex).ToList();
                result += GetBodyChildren(tagFieldNo, info, model);
                result += "</" + tagFieldCode + ">";
            }
            return result;
        }

        private string GetBodyChildren(Guid id, List<RECP_BO_ProMetaDefineModel> baseInfo, Temp_ImportProData list)
        {
            var result = "";
            var infos = baseInfo.Where(x => x.ParentNo == id).ToList();
            if (infos.Count > 1)
            {
                foreach (var info in infos)
                {
                    var fieldNo = info.FieldNo;
                    var fieldCode = info.FieldCode;
                    result += "<" + fieldCode + " " + GetNodeAttrInfo(fieldNo) + ">";
                    result += GetBodyChildren(fieldNo, baseInfo, list);
                    result += "</" + fieldCode + ">";
                }
            }
            else
            {
                var code = baseInfo.Where(x => x.FieldNo == id).ToList()[0].FieldCode;
                var Properties = list.GetType().GetProperties();
                foreach (var p in Properties)
                {
                    var name = p.Name;
                    if (name == code)
                    {
                        result = p.GetValue(list, null) != null ? p.GetValue(list, null).ToString() : "";
                    }
                }
            }
            return result;
        }
    }
}
