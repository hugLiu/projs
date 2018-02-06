using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class BO_ThreeLevReserveModel
    {
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        public string BOT { get; set; }

        public string SID { get; set; }

        public string Name { get; set; }

        public string BOC { get; set; }

        public int? OrderIndex { get; set; }

        public string Year { get; set; }

        public string YearStart { get; set; }

        public string YearEnd { get; set; }

        public string Category { get; set; }

        public string Series { get; set; }

        public string Formation { get; set; }

        public string Layer { get; set; }

        public string BlockGuan { get; set; }
    }
}
