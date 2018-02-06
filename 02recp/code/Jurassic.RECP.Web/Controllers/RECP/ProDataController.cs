using System;
using System.Web.Mvc;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using Jurassic.WebFrame;
using NPOI.SS.UserModel;
using Jurassic.RECP.DataService.Service;
using Newtonsoft.Json;
using NPOI.XSSF.UserModel;
using System.Configuration;
using System.Linq;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    [Authorize]
    public class ProDataController : BaseController
    {
        /// <summary>
        /// 服务层接口
        /// </summary>
        private ICommonService commonService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ICommonService">公共方法库</param>
        public ProDataController(ICommonService ICommonService)
        {
            commonService = ICommonService;
        }

        /// <summary>
        /// Excel导入页
        /// </summary>
        /// <returns></returns>
        public ActionResult Importing(string metaEntityCN, string metaEntity, int start, string operSN)
        {
            ViewBag.metaEntityCN = metaEntityCN;
            ViewBag.metaEntity = metaEntity;
            ViewBag.start = start;
            ViewBag.operSN = operSN;
            return View();
        }

        /// <summary>
        /// 加载表格数据，生成表头信息 & 创建Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="metaEntity"></param>
        /// <returns></returns>
        public JsonResult LoadTableInfo(string fileName, string metaEntity)
        {
            //获取表头信息
            //commonService.GetGroupHeaderStr(metaEntity);
            //创建Excel
            commonService.CreateExcel(fileName, metaEntity);
            //读取Excel文件
            //var result=ReadExcel(fileName);
            //return Json(result, JsonRequestBehavior.AllowGet);

            InitializeWorkbook(fileName);
            var headerCount = sheet.LastRowNum - sheet.FirstRowNum + 1;
            DataTable ddt = ConvertToDataTable(); //拿到Excel的所有数据（通过DataTable的形式存储）
            var countRow = ddt.Rows.Count; //包括表头在内的总行数
            var countCol = ddt.Columns.Count;
            string[,] headerData = new string[headerCount, countCol];
            for (var i = 0; i < headerCount; i++)
            {
                for (var j = 0; j < countCol; j++)
                {
                    var demo = "";
                    if (ddt.Rows[i][j] == System.DBNull.Value)
                    {
                        demo = "";
                    }
                    else
                    {
                        demo = (string)ddt.Rows[i][j];
                    }
                    headerData[i, j] = demo;
                }
            }

            string[,] bodyData = new string[countRow - headerCount + 1, countCol];
            for (var i = headerCount; i < countRow; i++)
            {
                for (var j = 0; j < countCol; j++)
                {
                    var demo = "";
                    if (ddt.Rows[i][j] == System.DBNull.Value)
                    {
                        demo = "";
                    }
                    else
                    {
                        demo = (string)ddt.Rows[i][j];
                    }
                    bodyData[i - headerCount, j] = demo;
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer jsSeria = new System.Web.Script.Serialization.JavaScriptSerializer();
            string _headerData = jsSeria.Serialize(headerData);
            string _bodyData = jsSeria.Serialize(bodyData);

            ExcelModel em = new ExcelModel();
            em.colCount = countCol;
            em.rowCount = countRow;
            em.headerData = _headerData;
            em.bodyData = _bodyData;
            return Json(em, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 声明 HSSFWorkbook 对象
        /// </summary>
        private static HSSFWorkbook hssfworkbook;  //是操作Excel2003（包括2003)版本，扩展名是.xls
        private static XSSFWorkbook xssfworkbook; //是操作Excel2007版本，扩展名是.xlsx
        private static ISheet sheet;
        /// <summary>
        /// 声明 HSSFSheet 对象
        /// </summary>
        //private static HSSFSheet _sheet;

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 本地化数据（管理页--专业数据采集---对象树）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProTreeData()
        {
            var url = Server.MapPath("/") + ("/Local/RECP/Json/ProMetadata/MetaDataEntityClass.json");
            var d = JsonConvert.ToString(System.IO.File.ReadAllText(url));
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载Excel文件
        /// </summary>
        /// <returns></returns>
        public JsonResult DownLoadTempExcel(string fileName)
        {
            var configPath = ConfigurationManager.AppSettings["FileFolderPath"] != null ? ConfigurationManager.AppSettings["FileFolderPath"].ToString() : "";
            var filePath = configPath + fileName + ".xls";
            DonwnLoadFile(filePath);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 具体下载Excel文件的实现方法
        /// </summary>
        /// <param name="relativPath">文件的相对路径</param>
        public void DonwnLoadFile(string relativPath)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(relativPath); //路径
            if (System.IO.File.Exists(path))  //如果文件存在
            {
                FileInfo fileInfo = new FileInfo(path);
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(relativPath.Substring(relativPath.LastIndexOf('/') + 1)));
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }
        /// <summary>
        /// 读Excel的数据传给前端
        /// </summary>
        /// <param name="headerCount">表头的行数</param>
        /// <param name="filePath">需要读取的Excel的地址</param>
        /// <returns></returns>
        public JsonResult ReadExcel(string filePath)
        {
            InitializeWorkbook(filePath);
            var headerCount = sheet.LastRowNum - sheet.FirstRowNum + 1;
            DataTable ddt = ConvertToDataTable(); //拿到Excel的所有数据（通过DataTable的形式存储）
            var countRow = ddt.Rows.Count; //包括表头在内的总行数
            var countCol = ddt.Columns.Count;
            string[,] headerData = new string[headerCount, countCol];
            for (var i = 0; i < headerCount; i++)
            {
                for (var j = 0; j < countCol; j++)
                {
                    var demo = "";
                    if (ddt.Rows[i][j] == System.DBNull.Value)
                    {
                        demo = "";
                    }
                    else
                    {
                        demo = (string)ddt.Rows[i][j];
                    }
                    headerData[i, j] = demo;
                }
            }

            string[,] bodyData = new string[countRow - headerCount, countCol];
            for (var i = headerCount; i < countRow; i++)
            {
                for (var j = 0; j < countCol; j++)
                {
                    var demo = "";
                    if (ddt.Rows[i][j] == System.DBNull.Value)
                    {
                        demo = "";
                    }
                    else
                    {
                        demo = (string)ddt.Rows[i][j];
                    }
                    bodyData[i - headerCount, j] = demo;
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer jsSeria = new System.Web.Script.Serialization.JavaScriptSerializer();
            string _headerData = jsSeria.Serialize(headerData);
            string _bodyData = jsSeria.Serialize(bodyData);

            ExcelModel em = new ExcelModel();
            em.colCount = countCol;
            em.rowCount = countRow;
            em.headerData = _headerData;
            em.bodyData = _bodyData;
            return Json(em, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 初始化Excel
        /// </summary>
        /// <param name="filePath"></param>
        public void InitializeWorkbook(string filePath)
        {
            var configPath = ConfigurationManager.AppSettings["FileFolderPath"] != null ? ConfigurationManager.AppSettings["FileFolderPath"].ToString() + filePath + ".xls" : "";
            string path = System.Web.HttpContext.Current.Server.MapPath(configPath);
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (filePath.Contains("xlsx"))
                {
                    xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
                else
                {
                    hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
            }
        }
        DataTable dt = new DataTable();
        /// <summary>
        /// 将Excel中的数据加到DataTable中
        /// </summary>
        /// <returns></returns>
        public DataTable ConvertToDataTable()
        {
            //ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            int colLength = sheet.GetRow(0).LastCellNum;
            for (int j = 0; j < colLength; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }
            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Excel 导入 文件检查
        /// </summary>
        /// <returns></returns>
        public JsonResult GetImportingResult(string metaEntity, int start, string operSN)
        {
            var upLoadFile = Request.Files["Fdata"];
            if (upLoadFile == null) return Json("null");
            var name = upLoadFile.FileName;
            var fileName= name.Substring(name.LastIndexOf("\\") + 1); 
            var filePath = Request.PhysicalApplicationPath + "FileCache\\" + fileName; 
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            upLoadFile.SaveAs(filePath);
            var guid = new Guid(operSN);
            //找到目标文件对象
            commonService.SaveExcelToTemp(filePath, metaEntity, start, guid);

            //删除清空文件夹
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            //返回在界面中
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excel 临时表数据保存到BO_BaseInfo 表
        /// </summary>
        /// <param name="operSN"></param>
        /// <param name="metaEntity"></param>
        /// <param name="rootId"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        public void SaveExcelTmpToBO(string operSN, string metaEntity, string rootId, string bot, string boc)
        {
            operSN = HttpUtility.UrlDecode(operSN);
            rootId = HttpUtility.UrlDecode(rootId);
            bot = HttpUtility.UrlDecode(bot);
            boc = HttpUtility.UrlDecode(boc);
            var opSN = new Guid(operSN);
            var rId = new Guid(rootId);
            var userName = CurrentUser.Name;
            commonService.SaveExcelTmpToBO(opSN, metaEntity, userName, rId, bot, boc);
        }

        public void DeleteTempData(string operSN)
        {
            operSN = HttpUtility.UrlDecode(operSN);
            var opSN = new Guid(operSN);
            commonService.DeleteTempProData(opSN);
        }

        /// <summary>
        /// 获取Excel导入专业数据
        /// </summary>
        /// <param name="metaEntity"></param>
        /// <returns></returns>
        public JsonResult GetTempImportProData()
        {
            var metaEntity = Request["metaEntity"];
            var operSN = Request["operSN"];
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            metaEntity = HttpUtility.UrlDecode(metaEntity);
            operSN = HttpUtility.UrlDecode(operSN);
            var opSN = new Guid(operSN);
            var list = commonService.GetV_TempImportProData(new V_Temp_ImportProDataModel { MetaEntity = metaEntity,OperSN = opSN });
            if (list == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            var data = new
            {
                data = list.Skip(pageIndex * pageSize).Take(pageSize),
                total = list.Count()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    /// <summary>
    /// 传给前端Excel数据的模板
    /// </summary>
    public class ExcelModel
    {
        public int colCount { get; set; }  //总列数
        public int rowCount { get; set; }   //总行数
        public string headerData { get; set; }   //表头数据
        public string bodyData { get; set; }   //表格数据
    }
}