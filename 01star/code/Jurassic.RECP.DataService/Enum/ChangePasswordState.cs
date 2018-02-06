using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Enum
{
    public enum ChangePasswordState
    {
        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,
        /// <summary>
        /// 旧密码错误
        /// </summary>
        OldPasswordError,
        /// <summary>
        /// 修改密码成功
        /// </summary>
        OK
    }
}
