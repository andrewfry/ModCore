using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.DataAccess
{
    public interface IDataRepositoryAsync<T> where T : BaseEntity
    {

        Task InsertAsync(T entity);

        Task InsertAsync(ICollection<T> entities);

        Task<T> UpdateAsync(T entity);

        Task UpdateAsync(ICollection<T> entities);

        Task DeleteAllAsync(ISpecification<T> specification);

        Task DeleteAsync(ISpecification<T> specification);

        Task<ICollection<T>> FindAllAsync(ISpecification<T> specification);

        Task<T> FindAsync(ISpecification<T> specification);

        Task<T> FindByIdAsync(string id);

        Task<IPagedResult<T>> FindAllByPageAsync(ISpecification<T> specification, IPagedRequest request);
    }
}
