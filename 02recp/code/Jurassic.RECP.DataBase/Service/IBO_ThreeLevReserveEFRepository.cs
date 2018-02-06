using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework; 
using Jurassic.RECP.DataBase.Models.View;

namespace Jurassic.RECP.DataBase.Service
{
    public interface IBO_ThreeLevReserveEFRepository : IEntityFrameworkRepository<v_BO_ThreeLevReserve>
    {
    }
}
