using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Enum
{
    public enum RemoveFavCatalog
    {
        /// <summary>
        /// 文件夹为空，移除成功
        /// </summary>
        RemoveSuccess,
        /// <summary>
        /// 文件夹不为空，移除失败
        /// </summary>
        RemoveFailure
    }

}
