using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class webpages_OAuthMembershipEFRepository : EntityFrameworkRepository<webpages_OAuthMembership>, Iwebpages_OAuthMembershipEFRepository
    {
        public webpages_OAuthMembershipEFRepository(GeoDbContext context) : base(context) { }
    }
}
