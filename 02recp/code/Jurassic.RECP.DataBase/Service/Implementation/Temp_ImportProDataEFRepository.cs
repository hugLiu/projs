using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase.Service
{
    public class Temp_ImportProDataEFRepository : EntityFrameworkRepository<Temp_ImportProData>, ITemp_ImportProDataEFRepository
    {
        public Temp_ImportProDataEFRepository(GeoDbContext context) : base(context) { }
    }
}
