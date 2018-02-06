using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class RECP_BO_ProMetaDefineModel
    {
        public Guid FieldNo { get; set; }

        public string MetaEntity { get; set; }

        public string FieldCode { get; set; }

        public string FieldCaption { get; set; }

        public Guid? ParentNo { get; set; }

        public int FieldIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public string Remark { get; set; }

        public int State { get; set; }
    }
}
