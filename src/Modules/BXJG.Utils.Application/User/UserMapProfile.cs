using System.Linq;
using AutoMapper;
using Abp.Authorization;
using BXJG.Utils.Localization;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using Abp.Authorization.Users;
using BXJG.Utils.Application.Share.User;

namespace BXJG.Utils.Application.User
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            /*
             * 具体的项目使用IncludeBase来指向这里的映射。
             * 
             * 这里是配置AbpUserBase，其实中间还有一层，参考 UserMapExt扩展方法
             */

            CreateMap<UserEditDto, AbpUserBase>();
            CreateMap<UserCreateDto, AbpUserBase>()/*.IncludeBase<UserEditDto,AbpUserBase>()*/;

            CreateMap<AbpUserBase, UserSelectDto>();

            CreateMap<AbpUserBase, UserEditDto>();
            CreateMap<AbpUserBase, UserCreateDto>()/*.IncludeBase<AbpUserBase, UserEditDto>()*/;
            CreateMap<AbpUserBase, UserDto>().IncludeBase<AbpUserBase, UserSelectDto>();//.IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeGetTreeNodeBaseDto<>));
        }
    }
}