namespace Jurassic.RECP.DataBase.Models.View
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class v_BO_WellTest
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

        [Column(TypeName = "xml")]
        public string ObjectParam { get; set; }

        public string Parents { get; set; }

        public string BlockGuan { get; set; }

        public string BlockType { get; set; }
    }
}
