using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class GT_CodeModel
    {
        public int Id { get; set; }

        public int CodeTypeId { get; set; }
 
        public string Code { get; set; }
 
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }
 
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
 
        public string UpdatedBy { get; set; } 
    }
}
