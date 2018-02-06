using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    /// <summary>
    /// 对象属性接口
    /// </summary>
    public interface IBO_BaseInfoService
    { 
        /// <summary>
        /// 获取对象属性接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<BO_BaseInfoModel> GetBO_BaseInfo(BO_BaseInfoModel model);

        /// <summary>
        /// 编辑对象属性接口
        /// </summary>
        /// <param name="models"></param>
        void ModifyBO_BaseInfo(List<BO_BaseInfoModel> models);

        /// <summary>
        /// 更新对象属性接口
        /// </summary>
        /// <param name="model"></param>
        /// <param name="baseFeatureModel"></param>
        void UpdateBO_BaseInfo(BO_BaseInfoModel model, BO_BaseFeatureModel baseFeatureModel);
    }
}
