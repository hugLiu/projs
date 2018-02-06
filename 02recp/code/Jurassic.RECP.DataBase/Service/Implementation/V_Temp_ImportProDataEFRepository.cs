using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service 
{
    public class V_Temp_ImportProDataEFRepository :  EntityFrameworkRepository<V_Temp_ImportProData>, IV_Temp_ImportProDataEFRepository
    {
        public V_Temp_ImportProDataEFRepository(GeoDbContext context) : base(context) { }
    }
}
