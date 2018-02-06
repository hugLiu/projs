using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class BO_BOAliasModel
    {
        public Guid AliasID { get; set; }

        public Guid BOID { get; set; }
         
        public string Alias { get; set; }

        public DateTime? CreatedDate { get; set; }
 
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
 
        public string LastUpdatedBy { get; set; }
 
        public string Remark { get; set; }
 
        public string SourceID { get; set; }
 
        public string SourceDB { get; set; }
 
        public string SourceTable { get; set; }

        public string _state { get; set; }
    }
}
