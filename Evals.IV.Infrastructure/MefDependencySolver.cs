using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Evals.IV.Infrastructure
{
    public class MefDependencySolver : IDependencyResolver
    {

        private readonly ComposablePartCatalog _catalog;
        private const string HttpContextKey = "MefContainerKey";
        private bool IsUpdate { get; set; }
        public MefDependencySolver(ComposablePartCatalog catalog, bool isupdate)
        {
            _catalog = catalog;
            this.IsUpdate = isupdate;
        }

        public CompositionContainer Container
        {
            get
            {
                if (IsUpdate)
                {
                    HttpContext.Current.Items.Remove(HttpContextKey);
                }
                if (!HttpContext.Current.Items.Contains(HttpContextKey))
                {
                    HttpContext.Current.Items.Add(HttpContextKey, new CompositionContainer(_catalog));
                }
                CompositionContainer container = (CompositionContainer)HttpContext.Current.Items[HttpContextKey];
                HttpContext.Current.Application["Container"] = container;
                return container;
            }
        }

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            try
            {
                string contractName = AttributedModelServices.GetContractName(serviceType);
                return Container.GetExportedValueOrDefault<object>(contractName);
            }
            catch (Exception ex)
            {
                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    using (var sw = new StreamWriter(HttpContext.Current.Server.MapPath("/app_data/a.txt"))) 
                    {
                        foreach (var item in loaderExceptions)
                        {
                            sw.WriteLine(item.Message);
                        }
                        sw.Close();
                    }
                }
                throw ex;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetExportedValues<object>(serviceType.FullName);
        }

        #endregion
    }

}
