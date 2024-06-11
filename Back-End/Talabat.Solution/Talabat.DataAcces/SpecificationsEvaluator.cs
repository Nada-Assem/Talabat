using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.DataAcces
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> Spec)
        {
            var query = inputQuery;
            if (Spec.Criteria is not null)
                query = query.Where(Spec.Criteria);

            query = Spec.Includes.Aggregate(query, 
                (CurrentQuery, ExpressionIncludes)
                => CurrentQuery.Include(ExpressionIncludes));

            return query;
        }
    }
}
