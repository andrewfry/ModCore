using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.DataAccess
{
   public  interface IDataRepository<T> where T : class
    {

        void Insert(T entity);

        void Insert(ICollection<T> entities);

        void Update(T entity);

        void Update(ICollection<T> entities);

        ICollection<T> DeleteAll(ISpecification<T> specification);

        void Delete(ISpecification<T> specification);

        ICollection<T> FindAll(ISpecification<T> specification) ;

        T Find(ISpecification<T> specification);
    }
}
