using Jurassic.RECP.DataBase.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class UserBehaviorEFRepository : EntityFrameworkRepository<RECP_User_Behavior>, IUserBehaviorEFRepository
    {
        public UserBehaviorEFRepository(GeoDbContext context) : base(context){}

        //public IQueryable<RECP_User_Behavior> GetQueryDb()
        //{
        //    return this.GetQuery();
        //}
    }
}
