using System;

namespace Jurassic.RECP.DataService.Models
{
    public class GT_TopicCardModel
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public int? OrderId { get; set; }

        public string Title { get; set; }

        public string Definition { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }
    }
}
