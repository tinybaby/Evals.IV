using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evals.IV.Domain.DomainModules;
using Evals.IV.Infrastructure;

namespace Evals.IV.Domain.Repository.EntityFramework
{
    public interface IEFUnitOfWork :IUnitOfWork
    {
        void RegisterNew<TEntity>(TEntity entity) where TEntity : class,IEntity;

        void RegisterModified<TEntity>(TEntity entity) where TEntity : class,IEntity;

        void RegisterDeleted<TEntity>(TEntity entity) where TEntity : class,IEntity;
    }
}
