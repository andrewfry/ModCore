using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Services;
using ModCore.Abstraction.Site;
using ModCore.DataAccess.MongoDb;
using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Base
{
    public class BaseServiceAsync<T> : IServiceAsync<T> where T : BaseEntity
    {
        protected IDataRepositoryAsync<T> _repository;
        protected IMapper _mapper;
        protected ILog _logger;

        public BaseServiceAsync(IDataRepositoryAsync<T> repository, IMapper mapper, ILog logger)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.FindByIdAsync(id);
        }

    }
}
