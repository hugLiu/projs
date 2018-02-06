namespace Jurassic.RECP.DataBase.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BO_BOAlias
    {
        [Key]
        public Guid AliasID { get; set; }

        public Guid BOID { get; set; }

        [Required]
        [StringLength(100)]
        public string Alias { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [StringLength(200)]
        public string SourceID { get; set; }

        [StringLength(200)]
        public string SourceDB { get; set; }

        [StringLength(200)]
        public string SourceTable { get; set; }
    }
}
