using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evals.IV.Domain.DomainModules;
using Evals.IV.Domain.Repository.EntityFramework;
using Evals.IV.Infrastructure;

namespace Evals.IV.Store.Controllers
{
    public interface MData
    {
        string UA { get; }
    }
    [Export]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog, true);
            DependencyResolver.SetResolver(solver);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MyRazorViewEngine());
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

       
    }
}