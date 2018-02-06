using System;

namespace Jurassic.RECP.DataService.Models
{
    public class BO_WellTestModel
    {
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        public string BOT { get; set; }

        public string SID { get; set; }

        public string Name { get; set; }

        public string BOC { get; set; }

        public int? OrderIndex { get; set; }

        public string ObjectParam { get; set; }

        public string Parents { get; set; }

        public string BlockGuan { get; set; }

        public string BlockType { get; set; }
    }
}
