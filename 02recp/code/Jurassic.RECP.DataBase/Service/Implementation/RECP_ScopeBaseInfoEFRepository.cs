using Jurassic.RECP.DataBase.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service
{
    public class RECP_ScopeBaseInfoEFRepository : EntityFrameworkRepository<RECP_ScopeBaseInfo>, IRECP_ScopeBaseInfoEFRepository
    {
        public RECP_ScopeBaseInfoEFRepository(GeoDbContext context) : base(context) { }
    }
}
