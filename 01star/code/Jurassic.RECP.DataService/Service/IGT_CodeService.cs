using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    public interface IGT_CodeService
    {
        /// <summary>
        /// 获取码表属性接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<GT_CodeModel> GetGT_CodeInfo(GT_CodeModel model);
    }
}
