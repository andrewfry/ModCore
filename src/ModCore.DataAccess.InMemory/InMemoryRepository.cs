using ModCore.Abstraction.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IDataRepository<T> where T : class
    {
        public InMemoryRepository()
        {
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(ICollection<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ICollection<T> entities)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> DeleteAll(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

        public void Delete(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> FindAll(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

        public T Find(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

    }
}
