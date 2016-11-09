using AutoMapper;
using ModCore.Models.Access;
using ModCore.ViewModels.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Mappings
{
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            //Access
            CreateMap<User, RegisterViewModel>();
            CreateMap<RegisterViewModel, User>();
            CreateMap<LoginViewModel, User>();
        }
    }
}
