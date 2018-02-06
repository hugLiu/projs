using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework; 
using Jurassic.RECP.DataBase.Models.View;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class BO_ThreeLevReserveEFRepository : EntityFrameworkRepository<v_BO_ThreeLevReserve>, IBO_ThreeLevReserveEFRepository
    {
        public BO_ThreeLevReserveEFRepository(GeoDbContext context) : base(context) { }
    }
}
