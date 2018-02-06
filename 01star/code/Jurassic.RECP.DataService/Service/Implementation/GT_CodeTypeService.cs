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
    public class GT_CodeTypeService : IGT_CodeTypeService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IGT_CodeTypeEFRepository GT_CodeTypeEFRepository { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_IGT_CodeTypeEFRepository"></param>
        public GT_CodeTypeService(IGT_CodeTypeEFRepository _IGT_CodeTypeEFRepository)
        {
            GT_CodeTypeEFRepository = _IGT_CodeTypeEFRepository;
        }

        /// <summary>
        /// 获取码表类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<GT_CodeTypeModel> GetGT_CodeType(GT_CodeTypeModel model)
        {
            IEnumerable<GT_CodeType> info = GT_CodeTypeEFRepository.GetQuery();
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                info = info.Where(x => x.Code == model.Code);
            }
            return info.Select(Mapper.Map<GT_CodeType, GT_CodeTypeModel>).ToList();
        }
    }
}
