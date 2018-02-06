using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework; 
using Jurassic.RECP.DataBase.Models.View;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class BO_WellPointEFRepository : EntityFrameworkRepository<v_BO_WellPoint>, IBO_WellPointEFRepository
    {
        public BO_WellPointEFRepository(GeoDbContext context) : base(context) { }
    }
}
