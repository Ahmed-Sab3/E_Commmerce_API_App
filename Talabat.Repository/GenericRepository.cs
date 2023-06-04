using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specicfications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        
        {
            //if (typeof(T) == typeof(Product))

            //    return (IEnumerable<T>)await _dbcontext.Products.Include(P => P.productBrand).Include(P => P.productType).ToListAsync();
            //else
                return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);

        }


        public async Task<IReadOnlyList<T>> GetAllwithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }


        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task Add(T Entity)
        => await _dbcontext.Set<T>().AddAsync(Entity);


        public async void Update(T Entity)
        => _dbcontext.Set<T>().Update(Entity);

        public void Delete(T Entity)
        {
            _dbcontext.Set<T>().Remove(Entity);
        }
    }
}
