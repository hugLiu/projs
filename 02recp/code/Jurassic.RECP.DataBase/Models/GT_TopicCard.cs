namespace Jurassic.RECP.DataBase.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class GT_TopicCard
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public int? OrderId { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        public string Definition { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }
    }
}
