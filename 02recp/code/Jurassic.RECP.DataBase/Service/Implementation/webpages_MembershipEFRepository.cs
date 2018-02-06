using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class webpages_MembershipEFRepository : EntityFrameworkRepository<webpages_Membership>, Iwebpages_MembershipEFRepository
    {
        public webpages_MembershipEFRepository(GeoDbContext context) : base(context) { }
    }
}
