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
using BXJG.Utils.Application.Share.OU;
using Abp.Organizations;

namespace BXJG.Utils.Application.OU
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            CreateMap<OUCreateDto, OrganizationUnit>();
            CreateMap<OUEditDto, OrganizationUnit>();

            //由于子元素是泛型，所以放扩展方法中了
            //CreateMap<OrganizationUnit, OUSelectDto>();
            //CreateMap<OrganizationUnit, OUDto>();
        }
    }
}