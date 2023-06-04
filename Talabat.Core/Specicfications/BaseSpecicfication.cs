using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specicfications
{
    public class BaseSpecicfication<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; }= new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationeEnabled { get; set; }

        public BaseSpecicfication()
        {
          
        }
        public BaseSpecicfication(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
       
        }


        public void AddOderderBy(Expression<Func<T, object>>orderbyExpression) 
        {
            OrderBy = orderbyExpression;
        }

        public void AddOderderByDesc(Expression<Func<T, object>> orderbyDescExpression)
        {
            OrderByDesc = orderbyDescExpression;
        }

        public void ApplyPagination(int skip,int take) 
        {
            IsPaginationeEnabled= true;
            Skip=skip;
            Take=take;  

        
        }


    }
}
