using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase 
{
    public interface IBO_BaseInfoEFRepository : IEntityFrameworkRepository<BO_BaseInfo>
    {
    }
}
