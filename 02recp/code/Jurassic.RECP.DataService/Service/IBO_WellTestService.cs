using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Models.View;

namespace Jurassic.RECP.DataService.Service
{
    public interface IBO_WellTestService
    {
        IEnumerable<BO_WellTestModel> GetWellDataByXiZuCengNode(string name);
        /// <summary>
        /// 拿到BO_WellTest视图下所有的试油成果数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO_WellTestModel> GetWellTestResultGridData();
    }
}
