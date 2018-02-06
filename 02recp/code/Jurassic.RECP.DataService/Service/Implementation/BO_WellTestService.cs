using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Models.View;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataService.Models.View;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class BO_WellTestService : IBO_WellTestService
    {
        private IBO_WellTestEFRepository BO_WellTest { get; set; }
        public BO_WellTestService(IBO_WellTestEFRepository _BO_WellTest)
        {
            BO_WellTest = _BO_WellTest;
        }
        public IEnumerable<BO_WellTestModel> GetWellDataByXiZuCengNode(string name)
        {
            IEnumerable<v_BO_WellTest> BaseInfo = BO_WellTest.GetQuery().Where(x => x.Name == name);
            return BaseInfo.Select(Mapper.Map<v_BO_WellTest, BO_WellTestModel>).ToList();
        }
        /// <summary>
        /// 拿到BO_WellTest视图下所有的试油成果数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO_WellTestModel> GetWellTestResultGridData()
        {
            IEnumerable<v_BO_WellTest> result = BO_WellTest.GetQuery().Take(100);
            return result.Select(Mapper.Map<v_BO_WellTest,BO_WellTestModel>).ToList();
        }
    }
}
