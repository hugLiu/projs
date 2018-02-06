namespace Jurassic.RECP.DataBase.Models.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_BO_ThreeLevReserve
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        [StringLength(255)]
        public string BOT { get; set; }

        [StringLength(100)]
        public string SID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(255)]
        public string BOC { get; set; }

        public int? OrderIndex { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        public string Category { get; set; }

        public string Series { get; set; }

        public string Formation { get; set; }

        public string Layer { get; set; }

        public string BlockGuan { get; set; }
    }
}
