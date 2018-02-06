using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Models.View;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class BO_ThreeLevReserveService : IBO_ThreeLevReserveService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IBO_ThreeLevReserveEFRepository BO_ThreeLevReserveEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="_IBO_ThreeLevReserveEFRepository"></param>
        public BO_ThreeLevReserveService(IBO_ThreeLevReserveEFRepository _IBO_ThreeLevReserveEFRepository)
        {
            BO_ThreeLevReserveEFRepository = _IBO_ThreeLevReserveEFRepository;
        }

        /// <summary>
        /// 获取视图三级储量属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<BO_ThreeLevReserveModel> GetBaseInfoViewData(BO_ThreeLevReserveModel model)
        {
            IEnumerable<v_BO_ThreeLevReserve> BaseInfo = BO_ThreeLevReserveEFRepository.GetQuery();
            if (model.ID.ToString() != Guid.Empty.ToString())
            {
                BaseInfo = BaseInfo.Where(x => x.ID == model.ID);
            }
            if (!string.IsNullOrWhiteSpace(model.PID.ToString()))
            {
                BaseInfo = BaseInfo.Where(x => x.PID == model.PID);
            }
            if (!string.IsNullOrWhiteSpace(model.BOT))
            {
                BaseInfo = BaseInfo.Where(x => x.BOT == model.BOT);
            }
            if (!string.IsNullOrWhiteSpace(model.BOC))
            {
                BaseInfo = BaseInfo.Where(x => x.BOC == model.BOC);
            }
            if (!string.IsNullOrWhiteSpace(model.YearStart)&& !string.IsNullOrWhiteSpace(model.YearEnd))
            {
                BaseInfo = BaseInfo.Where(x => Convert.ToInt32(x.Year) >= Convert.ToInt32(model.YearStart)&& Convert.ToInt32(x.Year) <= Convert.ToInt32(model.YearEnd));
            } 
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                BaseInfo = BaseInfo.Where(x => x.Category == model.Category);
            }
            if (!string.IsNullOrWhiteSpace(model.Series))
            {
                BaseInfo = BaseInfo.Where(x => x.Series == model.Series);
            }
            if (!string.IsNullOrWhiteSpace(model.Formation))
            {
                BaseInfo = BaseInfo.Where(x => x.Formation == model.Formation);
            }
            if (!string.IsNullOrWhiteSpace(model.Layer))
            {
                BaseInfo = BaseInfo.Where(x => x.Layer == model.Layer);
            }
            if (!string.IsNullOrWhiteSpace(model.BlockGuan))
            {
                BaseInfo = BaseInfo.Where(x => x.BlockGuan == model.BlockGuan);
            }
            return BaseInfo.Select(Mapper.Map<v_BO_ThreeLevReserve, BO_ThreeLevReserveModel>).OrderBy(x => x.Name).ToList();
        }
    }
}
