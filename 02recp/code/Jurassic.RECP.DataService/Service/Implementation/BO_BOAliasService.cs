using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    public class BO_BOAliasService : IBO_BOAliasService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IBO_BOAliasEFRepository BO_BOAliasEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="_IBO_BOAliasEFRepository"></param>
        public BO_BOAliasService(IBO_BOAliasEFRepository _IBO_BOAliasEFRepository)
        {
            BO_BOAliasEFRepository = _IBO_BOAliasEFRepository;
        }

        /// <summary>
        /// 获取对象别名列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<BO_BOAliasModel> GetBOAlias(Guid id)
        {
            IEnumerable<BO_BOAlias> info = BO_BOAliasEFRepository.GetQuery(); 
            if (id.ToString() != Guid.Empty.ToString())
            {
                info = info.Where(x => x.BOID == id);
            } 
            return info.Select(Mapper.Map<BO_BOAlias, BO_BOAliasModel>).ToList();
        }

        /// <summary>
        /// 编辑对象别名
        /// </summary>
        /// <param name="models"></param>
        public void ModifyBO_BOAlias(List<BO_BOAliasModel> models)
        {
            foreach (var model in models)
            {
                if (model._state == EnumNodeState.added.ToString())
                {
                    AddBO_BOAlias(model);
                }
                if (model._state == EnumNodeState.modified.ToString())
                {
                    UpdateBO_BOAlias(model);
                }
                if (model._state == EnumNodeState.removed.ToString())
                {
                    RemoveBO_BOAlias(model);
                }
            }
        }

        /// <summary>
        /// 添加对象别名
        /// </summary>
        /// <param name="model"></param>
        public void AddBO_BOAlias(BO_BOAliasModel model)
        {
            var newModel = new BO_BOAlias
            {
                BOID = model.BOID, 
                AliasID = model.AliasID,
                Alias = model.Alias,
                CreatedDate = DateTime.Now,
                CreatedBy = model.CreatedBy
            };
            BO_BOAliasEFRepository.Add(newModel);
        }

        /// <summary>
        /// 更新特定对象别名
        /// </summary>
        /// <param name="model"></param>
        public void UpdateBO_BOAlias(BO_BOAliasModel model)
        {
            var alias = BO_BOAliasEFRepository.Find(t => t.BOID == model.BOID && t.AliasID == model.AliasID);
            if (alias == null) return;
            alias.Alias = model.Alias;
            alias.LastUpdatedBy = model.CreatedBy;
            alias.LastUpdatedDate = DateTime.Now;
            BO_BOAliasEFRepository.Update(alias);

        }

        /// <summary>
        /// 删除特定对象别名
        /// </summary>
        /// <param name="model"></param>
        public void RemoveBO_BOAlias(BO_BOAliasModel model)
        {
            var alias = BO_BOAliasEFRepository.Find(t => t.BOID == model.BOID && t.AliasID == model.AliasID);
            BO_BOAliasEFRepository.Delete(alias);
        }
    }
}
