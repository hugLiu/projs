using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Tool
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 创建模型映射
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<webpages_Membership, webpages_MembershipModel>();

            CreateMap<RECP_Album, RECP_AlbumModel>().ForMember(t => t._id, a => a.Ignore()).ForMember(t => t._pid, b => b.Ignore()).ForMember(t => t._state, c => c.Ignore());
            CreateMap<RECP_Video, RECP_VideoModel>().ForMember(t => t._id, a => a.Ignore()).ForMember(t => t._pid, b => b.Ignore()).ForMember(t => t._state, c => c.Ignore());
            CreateMap<GT_TopicCard, GT_TopicCardModel>();
        }
    }
}
