using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specicfications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity :BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec) 
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy); 
            }

            if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsPaginationeEnabled) query=query.Skip(spec.Skip).Take(spec.Take); 


            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludesExperssions) => CurrentQuery.Include(IncludesExperssions));

            return query;
        
        }

    }
}
