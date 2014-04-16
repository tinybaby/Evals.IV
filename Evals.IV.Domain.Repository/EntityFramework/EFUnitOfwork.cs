using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evals.IV.Infrastructure;

namespace Evals.IV.Domain.Repository.EntityFramework
{
    [Export(typeof(IEFUnitOfWork)), Export(typeof(IUnitOfWork))]
    internal class EFUnitOfwork : IEFUnitOfWork, IDisposable
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        DbContext DbContext { get; set; }

        public void RegisterNew<TEntity>(TEntity entity) where TEntity : class, DomainModules.IEntity
        {
            DbContext.Set<TEntity>().Add(entity);
            Committed = true;
        }

        public void RegisterModified<TEntity>(TEntity entity) where TEntity : class, DomainModules.IEntity
        {
            DbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            Committed = true;
        }

        public void RegisterDeleted<TEntity>(TEntity entity) where TEntity : class, DomainModules.IEntity
        {
            DbContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            Committed = true;
        }

        public bool Committed
        {
            get;
            private set;
        }

        public void Commit()
        {
            if (Committed)
                DbContext.SaveChanges();
        }

        public void Rollback()
        {
            Committed = false;
        }

        public void Dispose()
        {
            Commit();
            DbContext.Dispose();
            
        }
    }
}
