namespace Jurassic.RECP.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECP_BO_ProMetaAttribute
    {
        [Key]
        [Column(Order = 0)]
        public Guid FieldNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AttrCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AttrCaption { get; set; }

        [StringLength(100)]
        public string AttrValue { get; set; }

        public int AttrIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int State { get; set; }

        public virtual RECP_BO_ProMetaDefine RECP_BO_ProMetaDefine { get; set; }
    }
}
