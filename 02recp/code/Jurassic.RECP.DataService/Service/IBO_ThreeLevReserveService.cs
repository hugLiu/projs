using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{ 
    public interface IBO_ThreeLevReserveService
    {
        /// <summary>
        /// 获取三级储量视图数据接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<BO_ThreeLevReserveModel> GetBaseInfoViewData(BO_ThreeLevReserveModel model);
    }
}
