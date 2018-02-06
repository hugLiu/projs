using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class User_FavoriteCatalogModel
    {
        public Guid FavCatalogId { get; set; }

        public int UserId { get; set; }

        public string FavName { get; set; }

        public Guid? ParentCatalogId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}
