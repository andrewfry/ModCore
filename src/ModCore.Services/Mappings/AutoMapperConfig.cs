using AutoMapper;
using ModCore.Abstraction.Plugins;
using ModCore.Core.DataAccess;
using ModCore.Models.Access;
using ModCore.Models.Core;
using ModCore.Models.Page;
using ModCore.Models.Sessions;
using ModCore.Models.Site;
using ModCore.ViewModels.Access;
using ModCore.ViewModels.Admin.Plugin;
using ModCore.ViewModels.Core;
using ModCore.ViewModels.Site;
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
            CreateMap<User, vRegister>().ReverseMap();
            CreateMap<vLogin, User>();
            CreateMap<IPlugin, vPlugin>();
            CreateMap<Role, vRole>();
            CreateMap<SettingDescriptor, vSettingValue>();

            CreateMap<Log, vLog>();
            CreateMap<PagedResult<Log>, vPagedResult<vLog>>();

            CreateMap<UserActivity, vUserActivity>();
            CreateMap<PagedResult<UserActivity>, vPagedResult<vUserActivity>>();
            CreateMap<UserActivity, vUserActivityDetailed>()
                .ForMember(dest => dest.ViewName, opts => opts.MapFrom(src => src.Result.ViewName))
                .ForMember(dest => dest.ModelType, opts => opts.MapFrom(src => src.Result.ModelType))
                .ForMember(dest => dest.ResultType, opts => opts.MapFrom(src => src.Result.ResultType.ToString()))
                .ForMember(dest => dest.StatusCode, opts => opts.MapFrom(src => src.Result.StatusCode))
                .ForMember(dest => dest.AdditionalInfo, opts => opts.MapFrom(src => src.Result.AdditionalInfo));


            CreateMap<Menu, vMenu>();
        }
    }
}
