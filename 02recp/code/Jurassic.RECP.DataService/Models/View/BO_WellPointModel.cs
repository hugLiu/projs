using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models.View
{
    public class BO_WellPointModel
    {
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        public string BOT { get; set; }

        public string SID { get; set; }

        public string Name { get; set; }

        public string BOC { get; set; }

        public int? OrderIndex { get; set; }

        public string ObjectParam { get; set; }

        public string Elevation { get; set; }

        public string XCoordinate { get; set; }

        public string YCoordinate { get; set; }

        public string Parents { get; set; }

        public string BlockGuan { get; set; }

        public string BlockType { get; set; }
    }
}
