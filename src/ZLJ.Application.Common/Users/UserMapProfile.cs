using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.StaffInfo;

namespace ZLJ.Application.Common.Users
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            /*
             * 从utils扩展有两种方式
             * 
             * 一种是我们的dto都继承utis中对应的dto，此时需要增加字段时，我们多个dto都需要手动添加，
             * 但utils中添加的dto对本项目都有效。
             * 
             * 另一种是我们的dto按自己的继承方式，比如 新增和查询dto都继承 编辑模型。
             * 但需要实现utils中对应dto的接口。
             * 此时优缺点与前面的方式相反。
             * 
             * 目前使用第一种方式。
             */

            //CreateMap<UserDto, User>();
            //CreateMap<UserDto, User>()
            //    .ForMember(x => x.Roles, opt => opt.Ignore())
            //    .ForMember(x => x.CreationTime, opt => opt.Ignore());


            this.CreateUserEditMap<User>();
            CreateMap<ZLJ.Application.Common.Share.User.UserEditDto, User>().IncludeBaseEditUser()
                                                                            .ForMember(x => x.OrganizationUnits, x => x.Ignore())
                                                                            .ForMember(x => x.Roles, x => x.Ignore());

            this.CreateUserCreateMap<User>();
            CreateMap<ZLJ.Application.Common.Share.User.UserCreateDto, User>().IncludeBaseCreateUser();

            this.CreateUserSelectMap<User>();
            CreateMap< User, ZLJ.Application.Common.Share.User.UserProviderDto>().IncludeBaseSelectUser();

            this.CreateUserMap<User>();
            CreateMap<User, ZLJ.Application.Common.Share.User.UserDto>().IncludeBaseUser();


            //CreateMap<CreateUserDto, User>();
            //CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            //这个最好别用了
            CreateMap<User, UserDto>().IncludeBaseUser();//.IncludeBase<AbpUserBase, UserDto>();
            //CreateMap<User, ZLJ.Application.Common.Share.User.UserDto>().IncludeBase<AbpUserBase, UserDto>();
           
            //这个最好别用了
            CreateMap<StaffInfoEntity, ZLJ.Application.Common.Share.User.UserDto>().IncludeBase<User, ZLJ.Application.Common.Share.User.UserDto>();

            CreateMap<StaffInfoEntity, ZLJ.Application.Common.Share.User.UserProviderDto>().IncludeBase<User, ZLJ.Application.Common.Share.User.UserProviderDto>();
        }
    }
}
