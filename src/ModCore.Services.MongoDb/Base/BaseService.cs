using AutoMapper;
using Microsoft.Extensions.Options;
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
        protected MongoDbRepository<T> _repository;
        protected IOptions<MongoDbSettings> _dbSettings;
        protected IMapper _mapper;
        protected ILog _logger;

        public BaseServiceAsync(IOptions<MongoDbSettings> dbSettings, IMapper mapper, ILog logger)
        {
            _logger = logger;
            _mapper = mapper;
            _dbSettings = dbSettings;
            _repository = new MongoDbRepository<T>(dbSettings.Value.ConnectionString);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.FindByIdAsync(id);
        }

    }
}
