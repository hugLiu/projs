using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Enum
{
    public enum Collection
    {
        /// <summary>
        /// 收藏成功
        /// </summary>
        CollectSuccess,
        /// <summary>
        /// 已经被收藏
        /// </summary>
        HasCollected,
        /// <summary>
        /// 改变收藏的文件夹
        /// </summary>
        ChangeFavCatalogSuccess,
        /// <summary>
        /// 取消收藏
        /// </summary>
        CollectCancel
        
    }
}
