using AutoMapper;
using ModCore.Abstraction.Plugins;
using ModCore.Models.Access;
using ModCore.ViewModels.Access;
using ModCore.ViewModels.Admin.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.Mappings
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<LoginViewModel, User>();
            CreateMap<IPlugin, vPlugin>();
        }
    }
}
