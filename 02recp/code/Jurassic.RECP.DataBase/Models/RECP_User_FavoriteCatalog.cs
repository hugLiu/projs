namespace Jurassic.RECP.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECP_User_FavoriteCatalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RECP_User_FavoriteCatalog()
        {
            RECP_User_Behavior = new HashSet<RECP_User_Behavior>();
        }

        [Key]
        public Guid FavCatalogId { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string FavName { get; set; }

        public Guid? ParentCatalogId { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECP_User_Behavior> RECP_User_Behavior { get; set; }
    }
}
