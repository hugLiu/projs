using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class RECP_VideoModel
    {
        public string id { get; set; }
        
        public string pid { get; set; }
        
        public string name { get; set; }
        
        public string link { get; set; }
        
        public string url { get; set; }

        public int? fieldIndex { get; set; }

        public int _id { get; set; }
        public int _pid { get; set; }
        public string _state { get; set; }
    }
}
