using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service 
{
    public class V_Temp_ImportBOEFRepository : EntityFrameworkRepository<V_Temp_ImportBO>, IV_Temp_ImportBOEFRepository
    {
        public V_Temp_ImportBOEFRepository(GeoDbContext context) : base(context) { }
    }
}
