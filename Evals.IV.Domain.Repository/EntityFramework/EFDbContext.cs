using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evals.IV.Domain.DomainModules;

namespace Evals.IV.Domain.Repository.EntityFramework
{
    [Export(typeof(DbContext))]
    public class EFDbContext : DbContext
    {
        #region Ctor
        public EFDbContext()
            : base("name=DefaultConnection")
        {

        }


        #endregion


        #region Property


        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Configurations.Add(new ProductConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
