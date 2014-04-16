using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Evals.IV.Infrastructure;
using System.IO;
using System.Web.Http;
namespace Evals.IV.Store
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Register IOC Container
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog, false);
            DependencyResolver.SetResolver(solver);
            #endregion


            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MyRazorViewEngine());

            GlobalConfiguration.Configure(WebApiConfig.Register);

            #region 测试
            //File.Create(Server.MapPath("/App_Data/" + DateTime.Now.Ticks + ".dat"));

            #endregion

        }
    }
    public class MyRazorViewEngine : RazorViewEngine
    {
        public MyRazorViewEngine()
            : base()
        {
            AreaViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };
            AreaMasterLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/%/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };
            AreaPartialViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };
            ViewLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };
            MasterLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };
            PartialViewLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, partialPath);
        }
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext, viewPath, masterPath);
        }
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(controllerContext, virtualPath);
        }
    }

}
