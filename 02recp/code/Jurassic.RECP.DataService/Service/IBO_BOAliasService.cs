using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    /// <summary>
    /// 对象别名接口
    /// </summary>
    public interface IBO_BOAliasService
    {
        /// <summary>
        /// 获取对象别名列表接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<BO_BOAliasModel> GetBOAlias(Guid id);

        /// <summary>
        /// 编辑对象别名接口
        /// </summary>
        /// <param name="models"></param>
        void ModifyBO_BOAlias(List<BO_BOAliasModel> models);
    }
}
