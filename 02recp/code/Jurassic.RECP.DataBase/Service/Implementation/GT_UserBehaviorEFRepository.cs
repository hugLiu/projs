using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service
{
    public class GT_UserBehaviorEFRepository : EntityFrameworkRepository<GT_UserBehavior>, IGT_UserBehaviorEFRepository
    {
        public GT_UserBehaviorEFRepository(GeoDbContext context) : base(context) { }
    }
}
