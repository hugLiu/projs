using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Models
{
    public class User_BehaviorModel
    {
        public Guid RecNo { get; set; }

        public int UserId { get; set; }

        public string iiid { get; set; }

        public string EP_ProductType { get; set; }

        public string DC_Title_Text { get; set; }

        public string DC_Author { get; set; }

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

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public int State { get; set; }
    }
}
