using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Jurassic.RECP.DataBase;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataBase.Service.Implementation;
using Jurassic.RECP.DataService;
using Jurassic.RECP.DataService.Service;
using Jurassic.RECP.DataService.Service.Implementation;
using Jurassic.So.GeoTopic.Web;
using Jurassic.So.Infrastructure;
using Ninject;
using Ninject.Web.Common;

namespace Jurassic.RECP.Web
{
    public class MvcApplication : GTApplication
    {
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                var list = base.ControllerNameSpaces.ToList();
                //声明自身Controller所在的命名空间
                list.Insert(0, typeof(MvcApplication).Namespace + ".Controllers");
                return list;
            }
        }

        protected override void Application_Start()
        {
            base.Application_Start();

            //Todo: 额外的初始化代码
            AutoMapperUtil.LoadConfig();
        }

        /// <summary>
        /// 加入注入服务绑定
        /// </summary>
        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);

            //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
            //ninjectKernel.Rebind<ModelContext>().ToSelf()
            //.WithPropertyValue("Schema", "");

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "D:\\Temp"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<UserConfig>().ToSelf()
            //.WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
            //.WithPropertyValue("Theme", "blue");  //系统默认皮肤

            //Todo: 额外的注入代码
            //todo: 额外的注入代码
            ninjectKernel.Bind<GeoDbContext>().To<GeoDbContext>().InRequestScope();
            ninjectKernel.Bind<IGT_UserBehaviorEFRepository>().To<GT_UserBehaviorEFRepository>();
            ninjectKernel.Bind<IUserBehaviorEFRepository>().To<UserBehaviorEFRepository>();
            ninjectKernel.Bind<IUserBehaviorService>().To<UserBehaviorService>();
            ninjectKernel.Bind<Iwebpages_MembershipEFRepository>().To<webpages_MembershipEFRepository>();
            ninjectKernel.Bind<Iwebpages_MembershipService>().To<webpages_MembershipService>();
            ninjectKernel.Bind<IFavoriteCatalogService>().To<FavoriteCatalogService>();
            ninjectKernel.Bind<IGT_FavoriteCatalogEFRepository>().To<GT_FavoriteCatalogEFRepository>();
            ninjectKernel.Bind<IBO_ProMetaAttributeEFRepository>().To<BO_ProMetaAttributeEFRepository>();
            ninjectKernel.Bind<IBO_ProMetaDefineEFRepository>().To<BO_ProMetaDefineEFRepository>();
            ninjectKernel.Bind<IBO_ProMetaDefineService>().To<BO_ProMetaDefineService>();
            ninjectKernel.Bind<IBO_ProMetaAttributeService>().To<BO_ProMetaAttributeService>();

            ninjectKernel.Bind<IBO_BaseInfoService>().To<BO_BaseInfoService>();
            ninjectKernel.Bind<IBO_BaseInfoEFRepository>().To<BO_BaseInfoEFRepository>();
            ninjectKernel.Bind<IBO_BOAliasService>().To<BO_BOAliasService>();
            ninjectKernel.Bind<IBO_BOAliasEFRepository>().To<BO_BOAliasEFRepository>(); 

            ninjectKernel.Bind<IBO_ThreeLevReserveService>().To<BO_ThreeLevReserveService>();
            ninjectKernel.Bind<IBO_ThreeLevReserveEFRepository>().To<BO_ThreeLevReserveEFRepository>();

            ninjectKernel.Bind<IBO_WellPointEFRepository>().To<BO_WellPointEFRepository>();
            ninjectKernel.Bind<IBO_WellPointService>().To<BO_WellPointService>();

            ninjectKernel.Bind<IBO_WellTestEFRepository>().To<BO_WellTestEFRepository>();
            ninjectKernel.Bind<IBO_WellTestService>().To<BO_WellTestService>();

            ninjectKernel.Bind<ICommonService>().To<CommonService>();
            ninjectKernel.Bind<IV_Temp_ImportProDataEFRepository>().To<V_Temp_ImportProDataEFRepository>();
            ninjectKernel.Bind<IV_Temp_ImportBOEFRepository>().To<V_Temp_ImportBOEFRepository>();
            ninjectKernel.Bind<ITemp_ImportProDataEFRepository>().To<Temp_ImportProDataEFRepository>();

            ninjectKernel.Bind<IGT_CodeTypeService>().To<GT_CodeTypeService>();
            ninjectKernel.Bind<IGT_CodeTypeEFRepository>().To<GT_CodeTypeEFRepository>();
            ninjectKernel.Bind<IGT_CodeService>().To<GT_CodeService>();
            ninjectKernel.Bind<IGT_CodeEFRepository>().To<GT_CodeEFRepository>();

            ninjectKernel.Bind<IGT_TopicCardEFRepository>().To<GT_TopicCardEFRepository>();
            ninjectKernel.Bind<IGT_TopicCardService>().To<GT_TopicCardService>();

            ninjectKernel.Bind<IUserProfileEFRepository>().To<UserProfileEFRepository>();
            ninjectKernel.Bind<IUserProfileService>().To<UserProfileService>();

            ninjectKernel.Bind<IRECP_ScopeBaseInfoEFRepository>().To<RECP_ScopeBaseInfoEFRepository>();
            ninjectKernel.Bind<IRECP_ScopeBaseInfoService>().To<RECP_ScopeBaseInfoService>();
        }
    }
}
