using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Service;
using Jurassic.WebFrame;
using System;
using System.Web.Mvc;
using System.Linq;


namespace Jurassic.RECP.Web.Controllers.RECP
{
    /// <summary>
    /// 专业元数据
    /// </summary>
    public class ProMetadataController : BaseController
    {
        private IBO_ProMetaDefineService ProMetaDefine { get; set; }
        private IBO_ProMetaAttributeService ProMetaAttribute { get; set; }

        private IBO_ProMetaDefineEFRepository ProMetaDefineEFRepository { get; set; }
        /// <summary>
        /// 专业元数据操作
        /// </summary>
        /// <param name="proMetaDefine">专业元数据定义</param>
        /// <param name="proMetaAttribute">专业元数据属性</param>
        public ProMetadataController(IBO_ProMetaDefineService proMetaDefine, IBO_ProMetaAttributeService proMetaAttribute, IBO_ProMetaDefineEFRepository proMetaDefineEFRepository)
        {
            ProMetaDefine = proMetaDefine;
            ProMetaAttribute = proMetaAttribute;
            ProMetaDefineEFRepository = proMetaDefineEFRepository;
        }
        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// MetaTreeEdit视图
        /// </summary>
        /// <returns></returns>
        public ActionResult MetaTreeEdit(Guid fieldNo)
        {
            var proMetaDefine=ProMetaDefineEFRepository.GetQuery().Where(t=>t.FieldNo==fieldNo).ToList();
            ViewBag.fieldNo = fieldNo;
            ViewBag.fieldCode = proMetaDefine[0].FieldCode;
            ViewBag.fieldCaption = proMetaDefine[0].FieldCaption;
            return View();
        }

        /// <summary>
        /// 测试页面
        /// </summary>
        /// <returns></returns>
        //public ActionResult Test()
        //{
        //    return View();
        //}
        /// <summary>
        /// 获得专业元数据实体分类
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMetaDataEntity()
        { 
            var url = Server.MapPath("/") + ("/Local/RECP/Json/ProMetadata/MetaDataEntityClass.json");
            var d = System.IO.File.ReadAllText(url);
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除专业元数据实体
        /// </summary>
        /// <param name="json"></param>
        public void DeleteMetaDataEntity(string json)
        {
            var url = Server.MapPath("/") + ("/Local/RECP/Json/ProMetadata/MetaDataEntityClass.json");
            System.IO.File.WriteAllText(url,json);
        }
        /// <summary>
        /// 通过元数据实例获得元数据定义（对元数据定义表的操作）
        /// </summary>
        /// <param name="metaEntity">元数据实例</param>
        /// <returns></returns>
        public JsonResult GetProMetaDefineByMetaEntity(string metaEntity)
        {
            var result=ProMetaDefine.GetProMetaDefineByMetaEntity(metaEntity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过元数据定义表的字段流水号获得元数据属性（对元数据属性表的操作）
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <returns></returns>
        public JsonResult GetProMetaAttributeByFieldNo(Guid fieldNo)
        {
            var result = ProMetaAttribute.GetProMetaAttributeByFieldNo(fieldNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 增加元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        public void AddAttributeTable(Guid fieldNo, string attrCode, string attrCaption, string attrValue, int attrIndex)
        {
            ProMetaAttribute.AddMetaAttribute(fieldNo, attrCode, attrCaption, attrValue, attrIndex);
        }
        /// <summary>
        /// 更新元数据属性表中的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        /// <param name="attrCaption">属性名称</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="attrIndex">字段顺序</param>
        public void UpdateAttributeTable(Guid fieldNo,string attrCode,string attrCaption,string attrValue,int attrIndex)
        {
            ProMetaAttribute.UpdateMetaAttribute(fieldNo, attrCode, attrCaption, attrValue, attrIndex);
        }
        /// <summary>
        /// 删除元数据属性表的一行
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="attrCode">属性编码</param>
        public void DeleteAttributeTable(Guid fieldNo, string attrCode)
        {
            ProMetaAttribute.DeleteMetaAttribute(fieldNo, attrCode);
        }
        /// <summary>
        /// MetaAttribute 表格所有增删改查操作的保存方法
        /// </summary>
        /// <param name="fieldNo">字段流水号</param>
        /// <param name="nodeParams">拿到修改后的MetaAttribute的所有数据</param>
        public void SaveMetadata(Guid fieldNo,string nodeParams)
        {
            ProMetaAttribute.SaveMetadata(fieldNo,nodeParams);
        }
        /// <summary>
        /// 对专业元数据定义Tree的操作
        /// </summary>
        /// <param name="fieldNo">标识号</param>
        /// <param name="fieldCode">字段编码</param>
        /// <param name="fieldCaption">字段名称</param>
        public void UpdateMetaTree(Guid fieldNo,string fieldCode,string fieldCaption)
        {
            ProMetaDefine.UpdateMetaTree(fieldNo, fieldCode, fieldCaption);
        }
        /// <summary>
        /// 保存专业元数据定义树 包括添加树节点和删除树节点的操作
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="parentId">父ID</param>
        /// <param name="text">当前树节点text</param>
        /// <param name="fieldNo">标识符</param>
        /// <param name="metaEntity">元数据实体</param>
        /// <returns></returns>
        public string SaveMetaDataTree(string state,Guid parentId,string text,Guid fieldNo,string metaEntity)
        {
            var result=ProMetaDefine.SaveMetaDataTree(state, parentId, text, fieldNo, metaEntity);
            return result;
        }
        /// <summary>
        /// 专业元数据定义 获得FieldCaption tooltip的值
        /// </summary>
        /// <param name="metaEntity">元数据实体</param>
        /// <param name="fieldCode">字段编码</param>
        /// <returns></returns>
        public JsonResult GetFieldCaption(string metaEntity,string fieldCode)
        {
            var result = ProMetaDefine.GetFieldCaption(metaEntity, fieldCode);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}