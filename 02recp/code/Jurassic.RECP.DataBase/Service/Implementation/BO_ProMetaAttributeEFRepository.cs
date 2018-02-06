using Jurassic.RECP.DataBase.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class BO_ProMetaAttributeEFRepository : EntityFrameworkRepository<RECP_BO_ProMetaAttribute>, IBO_ProMetaAttributeEFRepository
    {
        public BO_ProMetaAttributeEFRepository(GeoDbContext context) : base(context){ }
    }
}
