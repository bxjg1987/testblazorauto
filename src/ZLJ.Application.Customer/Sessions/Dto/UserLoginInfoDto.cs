using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ZLJ.Authorization.Users;

namespace ZLJ.App.Admin.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : Common.Sessions.Dto.UserLoginInfoDto
    {
       
    }
}
