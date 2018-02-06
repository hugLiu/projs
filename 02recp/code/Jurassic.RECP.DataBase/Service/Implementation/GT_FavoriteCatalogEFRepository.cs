using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Jurassic.RECP.DataBase.Service.Implementation
{
    public class GT_FavoriteCatalogEFRepository : EntityFrameworkRepository<RECP_User_FavoriteCatalog>, IGT_FavoriteCatalogEFRepository
    {
        public GT_FavoriteCatalogEFRepository(GeoDbContext context) : base(context){}

        public IQueryable<RECP_User_FavoriteCatalog> GetQueryDb()
        {
            return this.GetQuery();
        }
    }
}
