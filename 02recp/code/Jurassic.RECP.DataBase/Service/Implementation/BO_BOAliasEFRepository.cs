using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service 
{
    public class BO_BOAliasEFRepository : EntityFrameworkRepository<BO_BOAlias>, IBO_BOAliasEFRepository
    {
        public BO_BOAliasEFRepository(GeoDbContext context) : base(context) { }
    } 
}
