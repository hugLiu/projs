using AutoMapper;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public class UserProfileService : IUserProfileService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IUserProfileEFRepository userProfileEFRepository { get; set; }

        /// <summary>
        /// 数据层构造方法注入
        /// </summary>
        /// <param name="_userProfileEFRepository"></param>
        public UserProfileService(IUserProfileEFRepository _userProfileEFRepository)
        {
            userProfileEFRepository = _userProfileEFRepository;
        }

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetRoleName(UserProfileModel model)
        {
            IEnumerable<UserProfile> info = userProfileEFRepository.GetQuery();
            info = info.Where(x => x.UserId == model.UserId);
            var name = info.First().webpages_Roles.First().RoleName;
            return name;
        }
    }
}
