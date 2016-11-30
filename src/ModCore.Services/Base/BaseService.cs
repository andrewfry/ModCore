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

namespace ModCore.Services.Base
{
    public class BaseService
    {
        protected IMapper _mapper;
        protected ILog _logger;

        public BaseService(IMapper mapper, ILog logger)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}
