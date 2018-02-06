namespace Jurassic.RECP.DataBase.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_Temp_ImportProData
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string MetaEntity { get; set; }

        [StringLength(255)]
        public string A { get; set; }

        [StringLength(255)]
        public string B { get; set; }

        [StringLength(255)]
        public string C { get; set; }

        [StringLength(255)]
        public string D { get; set; }

        [StringLength(255)]
        public string E { get; set; }

        [StringLength(255)]
        public string F { get; set; }

        [StringLength(255)]
        public string G { get; set; }

        [StringLength(255)]
        public string H { get; set; }

        [StringLength(255)]
        public string I { get; set; }

        [StringLength(255)]
        public string J { get; set; }

        [StringLength(255)]
        public string K { get; set; }

        [StringLength(255)]
        public string L { get; set; }

        [StringLength(255)]
        public string M { get; set; }

        [StringLength(255)]
        public string N { get; set; }

        [StringLength(255)]
        public string O { get; set; }

        [StringLength(255)]
        public string P { get; set; }

        [StringLength(255)]
        public string Q { get; set; }

        [StringLength(255)]
        public string R { get; set; }

        [StringLength(255)]
        public string S { get; set; }

        [StringLength(255)]
        public string T { get; set; }

        [StringLength(255)]
        public string U { get; set; }

        [StringLength(255)]
        public string V { get; set; }

        [StringLength(255)]
        public string W { get; set; }

        [StringLength(255)]
        public string X { get; set; }

        [StringLength(255)]
        public string Y { get; set; }

        [StringLength(255)]
        public string Z { get; set; }

        [StringLength(255)]
        public string AA { get; set; }

        [StringLength(255)]
        public string AB { get; set; }

        [StringLength(255)]
        public string AC { get; set; }

        [StringLength(255)]
        public string AD { get; set; }

        [StringLength(255)]
        public string AE { get; set; }

        [StringLength(255)]
        public string AF { get; set; }

        [StringLength(255)]
        public string AG { get; set; }

        [StringLength(255)]
        public string AH { get; set; }

        [StringLength(255)]
        public string AI { get; set; }

        [StringLength(255)]
        public string AJ { get; set; }

        [StringLength(255)]
        public string AK { get; set; }

        [StringLength(255)]
        public string AL { get; set; }

        [StringLength(255)]
        public string AM { get; set; }

        [StringLength(255)]
        public string AN { get; set; }

        [StringLength(255)]
        public string AO { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid OperSN { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExecState { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BOExist { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(4)]
        public string BOAction { get; set; }

        [StringLength(6)]
        public string ExecStateTxt { get; set; }
    }
}
