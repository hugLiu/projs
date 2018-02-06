using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service
{
    public class GT_CodeEFRepository : EntityFrameworkRepository<GT_Code>, IGT_CodeEFRepository
    {
        public GT_CodeEFRepository(GeoDbContext context) : base(context) { }
    }
}
