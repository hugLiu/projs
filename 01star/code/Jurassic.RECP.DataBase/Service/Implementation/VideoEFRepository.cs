using Jurassic.RECP.DataBase.EntityFramework;
using Jurassic.RECP.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataBase.Service
{ 
    public class VideoEFRepository : EntityFrameworkRepository<RECP_Video>, IVideoEFRepository
    {
        public VideoEFRepository(GeoDbContext context) : base(context) { }
    }
}
