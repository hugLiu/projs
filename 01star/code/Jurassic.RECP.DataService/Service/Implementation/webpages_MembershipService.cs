using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataService.Service.Implementation
{
    public class webpages_MembershipService : Iwebpages_MembershipService
    {
        private Iwebpages_MembershipEFRepository Webpages_Membership { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="webpages_Membership">Iwebpages_MembershipEFRepository实例</param>
        public webpages_MembershipService(Iwebpages_MembershipEFRepository webpages_Membership)
        {
            Webpages_Membership = webpages_Membership;
        }
        /// <summary>
        /// 根据当前用户Id获取所有与密码相关的一条记录
        /// </summary>
        /// <param name="_userId">当前用户Id</param>
        /// <returns></returns>
        public List<webpages_MembershipModel> GetPassMessByUserId(int _userId)
        {
            var PassMess=Webpages_Membership.GetQuery().Where(t => t.UserId == _userId).ToList();
            return PassMess.Select(AutoMapper.Mapper.Map<webpages_Membership, webpages_MembershipModel>).ToList();
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        public void ModifyUserPass(int userId, string password)
        {
            var passwordMess = Webpages_Membership.Find(t => t.UserId==userId);
            if (passwordMess == null) return;
            passwordMess.Password = password;
            Webpages_Membership.Update(passwordMess);
        }
    }
}
