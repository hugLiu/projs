using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service 
{
    public interface IGT_CodeTypeService
    {
        /// <summary>
        /// 获取码表类型接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<GT_CodeTypeModel> GetGT_CodeType(GT_CodeTypeModel model);
    }
}
