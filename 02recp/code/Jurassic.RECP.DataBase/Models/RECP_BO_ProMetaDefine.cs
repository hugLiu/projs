namespace Jurassic.RECP.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECP_BO_ProMetaDefine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RECP_BO_ProMetaDefine()
        {
            RECP_BO_ProMetaAttribute = new HashSet<RECP_BO_ProMetaAttribute>();
        }

        [Key]
        [Column(Order = 0)]
        public Guid FieldNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MetaEntity { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldCode { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldCaption { get; set; }

        public Guid? ParentNo { get; set; }

        public int FieldIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int State { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECP_BO_ProMetaAttribute> RECP_BO_ProMetaAttribute { get; set; }
    }
}
