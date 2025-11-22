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
using Abp.Organizations;
using BXJG.Utils.Application.Share.OU;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Core.OU;

namespace BXJG.Utils.Application.OU
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            

       
            this.CreateMapSelectOu<OUSelectDto>();

            CreateMap<OrganizationUnitEntity, ZLJ.Application.Common.Share.OU.OUSelectDto>().IncludeBaseSelectOu();
         
        }
    }
}