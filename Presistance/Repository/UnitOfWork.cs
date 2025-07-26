using Domin.Contract;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class UnitOfWork(ApplicationDbContexts contexts) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repostories = [];
        public IGenricRepo<T> GetRepository<T>() where T : class
        {
            if(repostories.ContainsKey(typeof(T).Name))
            {
                return (IGenricRepo<T>)repostories[typeof(T).Name];
            }
            else
            {
                var newRepo = new GenericRepo<T>(contexts);
                repostories.Add(typeof(T).Name, newRepo);
                return newRepo;
            }
        }

        public int SaveChanges()
        {
            return contexts.SaveChanges();
        }
    }
}
