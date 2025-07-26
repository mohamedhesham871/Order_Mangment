using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Contract
{
    public interface IUnitOfWork
    {
      public int SaveChanges();
      public IGenricRepo<T> GetRepository<T>() where T : class;
       
    }
}
