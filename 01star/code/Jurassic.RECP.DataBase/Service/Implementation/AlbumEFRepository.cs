using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service
{
    public class AlbumEFRepository : EntityFrameworkRepository<RECP_Album>, IAlbumEFRepository
    {
        public AlbumEFRepository(GeoDbContext context) : base(context) { }
    }
}
