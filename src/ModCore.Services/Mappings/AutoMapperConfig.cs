using AutoMapper;
using ModCore.Abstraction.Plugins;
using ModCore.Models.Access;
using ModCore.Models.Core;
using ModCore.Models.Page;
using ModCore.Models.Sessions;
using ModCore.ViewModels.Access;
using ModCore.ViewModels.Admin.Plugin;
using ModCore.ViewModels.Core;
using ModCore.ViewModels.Page;
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
            CreateMap<SessionUserData, AuthenticationUser>().ReverseMap();
            CreateMap<User, AuthenticationUser>().ReverseMap();
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<LoginViewModel, User>();
            CreateMap<IPlugin, vPlugin>();
            CreateMap<Page, PageViewModel>().ReverseMap();

            CreateMap<SettingDescriptor, vSettingValue>();
        }
    }
}
