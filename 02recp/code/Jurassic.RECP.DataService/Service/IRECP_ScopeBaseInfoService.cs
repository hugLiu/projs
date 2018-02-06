using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface IRECP_ScopeBaseInfoService
    {
        IEnumerable<RECP_ScopeBaseInfoModel> GetScopeBaseInfo(RECP_ScopeBaseInfoModel model);
        void ModifyScopeBaseInfo(List<RECP_ScopeBaseInfoModel> models);
        void UpdateScopeBaseInfo(RECP_ScopeBaseInfoModel model, BO_BaseFeatureModel baseFeatureModel);
    }
}
