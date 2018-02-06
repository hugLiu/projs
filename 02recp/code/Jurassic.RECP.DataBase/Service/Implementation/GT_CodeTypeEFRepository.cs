using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service
{
    public class GT_CodeTypeEFRepository : EntityFrameworkRepository<GT_CodeType>, IGT_CodeTypeEFRepository
    {
        public GT_CodeTypeEFRepository(GeoDbContext context) : base(context) { }
    }
}
