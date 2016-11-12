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
        public AutoMapperProfileConfiguration()
        {
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<LoginViewModel, User>();
        }
    }
}
