using AutoMapper;
using ZLJ.App.Common.Users;
using ZLJ.App.Customer.StaffInfo;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.StaffInfo;
using ZLJ.Customer;

namespace ZLJ.App.Customer
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Ô±¹¤µµ°¸
            CreateMap<StaffInfoCreateDto, CustomerStaffInfoEntity>().IncludeBase<CreateUserDto, User>();
            CreateMap<StaffInfoEditDto, CustomerStaffInfoEntity>() .IncludeBase<EditUserDto, User>();
            CreateMap<CustomerStaffInfoEntity, StaffInfoDto>().IncludeBase<User, UserDto>();
            CreateMap<StaffInfoDto, StaffInfoEditDto>().ForMember(c => c.Password, c => c.MapFrom(d => d.EquipmentPwd));
            CreateMap<StaffInfoDto, StaffInfoCreateDto>().ForMember(c => c.Password, c => c.MapFrom(d => d.EquipmentPwd)).ForMember(c=>c.Birthday,c=>c.MapFrom(d=>d.Birthday.HasValue?(DateTime?)d.Birthday.Value.DateTime:null));
            CreateMap<StaffInfoCreateDto, StaffInfoEditDto>();
            #endregion
        }
    }
}