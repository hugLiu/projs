using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Models.View;
using Jurassic.RECP.DataService.Models.View;

namespace Jurassic.RECP.DataService.Tool
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 创建模型映射
        /// </summary>
        public AutoMapperProfile()
        { 
            CreateMap<RECP_User_Behavior, User_BehaviorModel>();
            CreateMap<webpages_Membership, webpages_MembershipModel>();
            CreateMap<RECP_User_FavoriteCatalog, User_FavoriteCatalogModel>();
            CreateMap<RECP_BO_ProMetaAttribute, RECP_BO_ProMetaAttributeModel>();
            CreateMap<RECP_BO_ProMetaDefine, RECP_BO_ProMetaDefineModel>();

            CreateMap<BO_BaseInfo, BO_BaseInfoModel>().ForMember(t => t._id, a => a.Ignore()).ForMember(t => t._pid, b => b.Ignore()).ForMember(t => t._state, c => c.Ignore());
            CreateMap<BO_BOAlias, BO_BOAliasModel>().ForMember(t => t._state, s => s.Ignore());
 
            CreateMap<v_BO_ThreeLevReserve, BO_ThreeLevReserveModel>().ForMember(t => t.YearStart, s => s.Ignore()).ForMember(t => t.YearEnd, s => s.Ignore());

            CreateMap<v_BO_WellPoint, BO_WellPointModel>();

            CreateMap<v_BO_WellTest, BO_WellTestModel>();

            CreateMap<GT_Code, GT_CodeModel>(); 
            CreateMap<GT_CodeType, GT_CodeTypeModel>();

            CreateMap<Temp_ImportProData, Temp_ImportProDataModel>();
            CreateMap<Temp_ImportProDataModel, Temp_ImportProData>();
            CreateMap<V_Temp_ImportBOModel, V_Temp_ImportBO>();
            CreateMap<V_Temp_ImportBO, V_Temp_ImportBOModel>();
            CreateMap<V_Temp_ImportProData, V_Temp_ImportProDataModel>();
            CreateMap<V_Temp_ImportProDataModel, V_Temp_ImportProData>();

            CreateMap<GT_TopicCard, GT_TopicCardModel>();

            CreateMap<RECP_ScopeBaseInfoModel, RECP_ScopeBaseInfo>();
            CreateMap<RECP_ScopeBaseInfo, RECP_ScopeBaseInfoModel>().ForMember(t => t._id, a => a.Ignore()).ForMember(t => t._pid, b => b.Ignore()).ForMember(t => t._state, c => c.Ignore()); ;
        }
    }
}
