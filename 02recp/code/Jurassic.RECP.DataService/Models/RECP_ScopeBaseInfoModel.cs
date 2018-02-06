using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class RECP_ScopeBaseInfoModel
    {
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        public string BOT { get; set; }

        public string SID { get; set; }

        public string Name { get; set; }

        public string BOC { get; set; }

        public DbGeometry SpaceLocation { get; set; }

        public int? OrderIndex { get; set; }

        public string ObjectParam { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }

        public int _id { get; set; }
        public int _pid { get; set; }
        public string _state { get; set; }
    }
}
