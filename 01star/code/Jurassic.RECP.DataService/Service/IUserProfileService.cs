using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{ 
    public interface IUserProfileService
    {
       string GetRoleName(UserProfileModel model);
    }
}
