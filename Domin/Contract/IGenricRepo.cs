using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Contract
{
    public  interface IGenricRepo<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllIncludingAsync(
                  Expression<Func<T, bool>> predicate,
                  Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
        


    }
}
