using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service
{
    public interface IGT_FavoriteCatalogEFRepository : IEntityFrameworkRepository<RECP_User_FavoriteCatalog>
    {
        IQueryable<RECP_User_FavoriteCatalog> GetQueryDb();
    }
}
