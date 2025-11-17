using AutoMapper;
using ZLJ.Application.Share.StaffInfo;

namespace ZLJ.Admin.CoreRCL.Staffinfo
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StaffInfoDto, StaffInfoEditDto>();
        }
    }
}