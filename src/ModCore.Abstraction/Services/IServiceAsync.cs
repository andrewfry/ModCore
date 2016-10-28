using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModCore.Abstraction.Services
{
    public interface IServiceAsync<T>
    {
        Task<T> GetByIdAsync(string id);
    }
}
