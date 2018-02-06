using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Enum
{
    public enum DeleteMetaAttribute
    {
        /// <summary>
        /// 删除成功
        /// </summary>
        RemoveSuccess,
        /// <summary>
        /// 删除失败
        /// </summary>
        RemoveFailure
    }
    public enum EditMetaAttribute
    {
        /// <summary>
        /// 修改成功
        /// </summary>
        EditSuccess,
        /// <summary>
        /// 修改失败
        /// </summary>
        EditFailure
    }
    public enum AddMetaAttribute
    {
        /// <summary>
        /// 添加成功
        /// </summary>
        AddSuccess,
        /// <summary>
        /// 添加失败
        /// </summary>
        AddFailure
    }

}
