namespace Jurassic.RECP.DataBase.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECP_Video
    {
        [StringLength(50)]
        public string id { get; set; }

        [StringLength(50)]
        public string pid { get; set; }

        [StringLength(200)]
        public string name { get; set; }

        [StringLength(200)]
        public string link { get; set; }

        [StringLength(200)]
        public string url { get; set; }

        public int? fieldIndex { get; set; }
    }
}
