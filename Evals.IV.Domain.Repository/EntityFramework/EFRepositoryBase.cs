using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Evals.IV.Domain.DomainModules;
using Evals.IV.Domain.Specifications;
using Evals.IV.Infrastructure;

namespace Evals.IV.Domain.Repository.EntityFramework
{
    [Export]
    public class EFRepositoryBase<TAggregateRoot> : Repository<TAggregateRoot>, IDisposable where TAggregateRoot : class, IAggregateRoot
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        IEFUnitOfWork UnitOfWork { get; set; }

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        EFDbContext DbContext { get; set; }

        #region Private
        private MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        private string GetEagerLoadingPath(Expression<Func<TAggregateRoot, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty);
            var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }
        #endregion

        #region Member


        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            UnitOfWork.RegisterNew<TAggregateRoot>(aggregateRoot);

        }

        protected override TAggregateRoot DoGetByKey(Guid key)
        {
            return DbContext.Set<TAggregateRoot>().Find(key);
        }



        protected override IEnumerable<TAggregateRoot> DoGetAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return results;
        }

        protected override PagedResult<TAggregateRoot> DoGetAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, int skip, int take)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, skip, take);
            if (results == null || results == PagedResult<TAggregateRoot>.Empty || results.Data.Count() == 0)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return results;
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder)
        {
            var query = DbContext.Set<TAggregateRoot>()
                .Where(specification.GetExpression());
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return query.OrderBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return query.OrderByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return query.ToList();
        }

        protected override PagedResult<TAggregateRoot> DoFindAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, int skip, int take)
        {

            if (take <= 0)
                throw new ArgumentOutOfRangeException("take", take, "每页大小必须大于或等于1。");

            var query = DbContext.Set<TAggregateRoot>()
                .Where(specification.GetExpression());


            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        var pagedGroupAscending = query.OrderBy(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
                        if (pagedGroupAscending == null)
                            return null;
                        return new PagedResult<TAggregateRoot>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + take - 1) / take, take, skip, pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.Descending:
                        var pagedGroupDescending = query.OrderByDescending(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
                        if (pagedGroupDescending == null)
                            return null;
                        return new PagedResult<TAggregateRoot>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + take - 1) / take, take, skip, pagedGroupDescending.Select(p => p).ToList());
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }

        protected override TAggregateRoot DoGet(ISpecification<TAggregateRoot> specification)
        {
            TAggregateRoot result = this.DoFind(specification);
            if (result == null)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return result;
        }

        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification)
        {
            return DbContext.Set<TAggregateRoot>().Where(specification.IsSatisfiedBy).FirstOrDefault();
        }

        protected override bool DoExists(ISpecification<TAggregateRoot> specification)
        {
            var count = DbContext.Set<TAggregateRoot>().Count(specification.IsSatisfiedBy);
            return count != 0;
        }

        protected override void DoRemove(TAggregateRoot aggregateRoot)
        {
            UnitOfWork.RegisterDeleted<TAggregateRoot>(aggregateRoot);
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            UnitOfWork.RegisterModified<TAggregateRoot>(aggregateRoot);
        }

        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = DbContext.Set<TAggregateRoot>();
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                return dbquery.Where(specification.GetExpression()).FirstOrDefault();
            }
            else
                return dbset.Where(specification.GetExpression()).FirstOrDefault();
        }

        protected override IEnumerable<TAggregateRoot> DoGetAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return results;
        }

        protected override PagedResult<TAggregateRoot> DoGetAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, int skip, int take, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, skip, take, eagerLoadingProperties);
            if (results == null || results == PagedResult<TAggregateRoot>.Empty || results.Data.Count() == 0)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return results;
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = DbContext.Set<TAggregateRoot>();
            IQueryable<TAggregateRoot> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
                queryable = dbset.Where(specification.GetExpression());

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return queryable.OrderBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return queryable.OrderByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return queryable.ToList();
        }



        protected override PagedResult<TAggregateRoot> DoFindAll<TKey>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TKey>> sortPredicate, SortOrder sortOrder, int skip, int take, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {

            if (take <= 0)
                throw new ArgumentOutOfRangeException("take", take, "每页大小必须大于或等于1。");

            var dbset = DbContext.Set<TAggregateRoot>();
            IQueryable<TAggregateRoot> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
                queryable = dbset.Where(specification.GetExpression());

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        var pagedGroupAscending = queryable.OrderBy(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupAscending == null)
                            return null;
                        return new PagedResult<TAggregateRoot>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + take - 1) / take, take, skip, pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.Descending:
                        var pagedGroupDescending = queryable.OrderByDescending(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupDescending == null)
                            return null;
                        return new PagedResult<TAggregateRoot>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + take - 1) / take, take, skip, pagedGroupDescending.Select(p => p).ToList());
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }

        protected override TAggregateRoot DoGet(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            TAggregateRoot result = this.DoFind(specification, eagerLoadingProperties);
            if (result == null)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return result;
        }

        #endregion

        public void Dispose()
        {
            UnitOfWork.Commit();

        }
    }
}
