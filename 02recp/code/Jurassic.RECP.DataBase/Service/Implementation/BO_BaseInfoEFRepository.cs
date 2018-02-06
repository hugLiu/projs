using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase
{
    public class BO_BaseInfoEFRepository : EntityFrameworkRepository<BO_BaseInfo>, IBO_BaseInfoEFRepository
    {
        public BO_BaseInfoEFRepository(GeoDbContext context) : base(context) { }
    }
}
