using Abp.Organizations;
using AutoMapper;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo;
using ZLJ.Organizations.Dto;

namespace ZLJ.Users.Dto
{
    public class OrganizationUnitMapProfile : Profile
    {
        public OrganizationUnitMapProfile()
        {
            CreateMap<OrganizationUnitEntity, OrganizationUnitDto>().ForMember(c=>c.Children, c=>c.Ignore());

            //CreateMap<OrganizationUnitDto, OrganizationUnit>()
            //    .ForMember(x => x.Roles, opt => opt.Ignore())
            //    .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<EditOrganizationUnitDto, OrganizationUnitEntity>();
            //CreateMap<EditOrganizationUnitDto, OrganizationUnit>().ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
