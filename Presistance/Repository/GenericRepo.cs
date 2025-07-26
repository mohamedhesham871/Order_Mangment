using Domin.Contract;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class GenericRepo<TEntity>(ApplicationDbContexts contexts) : IGenricRepo<TEntity> where TEntity : class
    {
        public void Add(TEntity entity)
        {
          contexts.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            contexts.Set<TEntity>().Remove(entity);
        }

        public  Task<List<TEntity>> GetAllAsync()
        {
          return   contexts.Set<TEntity>().ToListAsync();
        }       

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await contexts.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await contexts.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
             contexts.Set<TEntity>().Update(entity);
        }
    }
}
