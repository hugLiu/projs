using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    public class GT_CodeService : IGT_CodeService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IGT_CodeEFRepository GT_CodeEFRepository { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_IGT_CodeEFRepository"></param>
        public GT_CodeService(IGT_CodeEFRepository _IGT_CodeEFRepository)
        {
            GT_CodeEFRepository = _IGT_CodeEFRepository;
        }

        /// <summary>
        /// 获取码表属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<GT_CodeModel> GetGT_CodeInfo(GT_CodeModel model)
        {
            IEnumerable<GT_Code> info = GT_CodeEFRepository.GetQuery(); 
            if (!string.IsNullOrWhiteSpace(model.CodeTypeId.ToString()))
            {
                info = info.Where(x => x.CodeTypeId == model.CodeTypeId);
            } 
            return info.Select(Mapper.Map<GT_Code, GT_CodeModel>).ToList();
        }
    }
}
