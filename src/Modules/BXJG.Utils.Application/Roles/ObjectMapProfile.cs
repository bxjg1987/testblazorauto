using System.Linq;
using AutoMapper;
using Abp.Authorization;
using BXJG.Utils.Localization;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using Abp.Authorization.Users;
using BXJG.Utils.Application.Share.User;
using Abp.Authorization.Roles;
using BXJG.Utils.Application.Share.Roles;

namespace BXJG.Utils.Application.Roles
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            CreateMap<AbpRoleBase, RoleSelectDto>().ForMember(x=>x.DisplayText,x=>x.MapFrom(d=>d.DisplayName))
                                                   .ForMember(x => x.Value, x => x.MapFrom(d => d.Name))
                                                   .ForMember(x => x.IsSelected, x => x.MapFrom(d => d.IsDefault));
            
            CreateMap<AbpRoleBase, RoleDto>();

            CreateMap<RoleCreateDto, AbpRoleBase>();

            CreateMap<RoleEditDto, AbpRoleBase>();
        }
    }
}