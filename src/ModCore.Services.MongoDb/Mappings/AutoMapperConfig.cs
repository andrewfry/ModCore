using AutoMapper;
using ModCore.Models.Access;
using ModeCore.ViewModels.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Mappings
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //Access
            Mapper.Initialize(cfg => cfg.CreateMap<User, RegisterViewModel>());

        }
    }
}
