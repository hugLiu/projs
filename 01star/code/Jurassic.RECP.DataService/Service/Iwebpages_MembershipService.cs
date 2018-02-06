using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface Iwebpages_MembershipService
    {
        List<webpages_MembershipModel> GetPassMessByUserId(int _userId);
        void ModifyUserPass(int userId, string password);
    }
}
