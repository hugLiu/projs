using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class RECP_BO_ProMetaAttributeModel
    {
        public Guid FieldNo { get; set; }

        public string AttrCode { get; set; }

        public string AttrCaption { get; set; }

        public string AttrValue { get; set; }

        public int AttrIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public string Remark { get; set; }

        public int State { get; set; }
        public string _state { get; set; }
    }
}
