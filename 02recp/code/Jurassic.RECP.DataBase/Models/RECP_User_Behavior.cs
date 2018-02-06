namespace Jurassic.RECP.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECP_User_Behavior
    {
        [Key]
        [Column(Order = 0)]
        public Guid RecNo { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string iiid { get; set; }

        [Required]
        [StringLength(50)]
        public string EP_ProductType { get; set; }

        [StringLength(128)]
        public string DC_Title_Text { get; set; }

        [StringLength(50)]
        public string DC_Author { get; set; }

        [StringLength(1024)]
        public string DC_Description { get; set; }

        public DateTime DC_Date_Created { get; set; }

        public DateTime IndexedDate { get; set; }

        public int FlagLook { get; set; }

        public int CountLook { get; set; }

        public int FlagFavorite { get; set; }

        public Guid? FavCatalogId { get; set; }

        public int FlagDownload { get; set; }

        public int CountDownload { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int State { get; set; }

        public virtual RECP_User_FavoriteCatalog RECP_User_FavoriteCatalog { get; set; }
    }
}
