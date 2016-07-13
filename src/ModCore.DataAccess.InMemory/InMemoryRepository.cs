using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        private static List<T> _dataStore = new List<T>();

        public InMemoryRepository()
        {
        }

        public void Insert(T entity)
        {
            _dataStore.Add(entity);
        }

        public void Insert(ICollection<T> entities)
        {
            _dataStore.AddRange(entities);
        }

        public void Update(T entity)
        {
            _dataStore.Remove(entity);
            _dataStore.Add(entity);
        }

        public void Update(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _dataStore.Remove(entity);
            }
            _dataStore.AddRange(entities);
        }

        public void DeleteAll(ISpecification<T> specification)
        {
            foreach (var entity in _dataStore.Where(specification.Predicate()))
            {
                _dataStore.Remove(entity);
            }
        }

        public void Delete(ISpecification<T> specification)
        {
            var entity = _dataStore.FirstOrDefault(specification.Predicate());
            if(entity != null)
            {
                _dataStore.Remove(entity);
            }
        }

        public ICollection<T> FindAll(ISpecification<T> specification)
        {
            return _dataStore.Where(specification.Predicate()).ToList();
        }

        public T Find(ISpecification<T> specification)
        {
            return _dataStore.FirstOrDefault(specification.Predicate());
        }

    }
}
