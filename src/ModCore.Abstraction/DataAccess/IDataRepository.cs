using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.DataAccess
{
   public  interface IDataRepository<T> where T : BaseEntity
    {

        void Insert(T entity);

        void Insert(ICollection<T> entities);

        T Update(T entity);

        void Update(ICollection<T> entities);

        void DeleteAll(ISpecification<T> specification);

        void Delete(ISpecification<T> specification);

        ICollection<T> FindAll(ISpecification<T> specification) ;

        ICollection<T> FindAll();

        T Find(ISpecification<T> specification);
    }
}
