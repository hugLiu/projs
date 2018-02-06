using Jurassic.RECP.DataBase.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class BO_ProMetaDefineEFRepository : EntityFrameworkRepository<RECP_BO_ProMetaDefine> ,IBO_ProMetaDefineEFRepository
    {
        public BO_ProMetaDefineEFRepository(GeoDbContext context) : base(context){ } 
    }
}
