using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework; 
using Jurassic.RECP.DataBase.Models.View;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class BO_WellTestEFRepository : EntityFrameworkRepository<v_BO_WellTest>, IBO_WellTestEFRepository
    {
        public BO_WellTestEFRepository(GeoDbContext context) : base(context) { }
    }
}
