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
     

            ninjectKernel.Bind<IAlbumEFRepository>().To<AlbumEFRepository>();
            ninjectKernel.Bind<IAlbumService>().To<AlbumService>();

            ninjectKernel.Bind<IVideoEFRepository>().To<VideoEFRepository>();
            ninjectKernel.Bind<IVideoService>().To<VideoService>(); 
        }
    }
}
