using System;
using System.Collections.Generic;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    public interface ICommonService
    {  
        /// <summary>
        /// 创建Excel
        /// </summary>
        /// <param name="fileName">节点名称</param>
        /// <param name="metaEntity">过滤参数</param>
        void CreateExcel(string fileName, string metaEntity);

        /// <summary>
        /// Excel 转存临时数据表中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaEntity"></param>
        /// <param name="start">从第x行开始读Excel数据</param>
        /// <param name="operSN"></param>
        void SaveExcelToTemp(string filePath, string metaEntity, int start, Guid operSN);

        /// <summary>
        /// 获取Excel导入专业数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<V_Temp_ImportProDataModel> GetV_TempImportProData(V_Temp_ImportProDataModel model);

        /// <summary>
        /// Excel 临时表数据保存到BO_BaseInfo 表
        /// </summary>
        /// <param name="operSN"></param>
        /// <param name="metaEntity"></param>
        /// <param name="userName"></param>
        /// <param name="rId"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        void SaveExcelTmpToBO(Guid operSN, string metaEntity, string userName, Guid rId, string bot, string boc);

        void DeleteTempProData(Guid opSN);
    }
}
