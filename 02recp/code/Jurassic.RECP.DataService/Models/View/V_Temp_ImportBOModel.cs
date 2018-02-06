using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class V_Temp_ImportBOModel
    {
        public string MetaEntity { get; set; }

        public string Name { get; set; }

        public Guid OperSN { get; set; }

        public int BOExist { get; set; }
    }
}
