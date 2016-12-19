using AutoMapper;
using RoleBasedPermission.Plugin.Models;
using RoleBasedPermission.Plugin.ViewModels;

namespace RolePermission.Plugin.Mapping
{
    public class RoleBaseMapping : Profile
    {
        public RoleBaseMapping()
        {
            this.CreateMap<vPermissionDiscriptor, PermissionDiscriptor>().ReverseMap();
            this.CreateMap<vPermissionAreaDiscriptor, PermissionAreaDiscriptor>().ReverseMap();
            this.CreateMap<vPermissionControllerDiscriptor, PermissionControllerDiscriptor>().ReverseMap();
            this.CreateMap<vPermissionActionDiscriptor, PermissionActionDiscriptor>().ReverseMap();
        }
    }
}
