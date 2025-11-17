using BXJG.Common.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Share.StaffInfo;

/// <summary>
/// 后台管理员工的编辑模型
/// </summary>
public class StaffInfoCreateDto : StaffInfoEditDto// EntityDto<long>
{
    public ZLJ.Application.Common.Share.User.UserCreateDto BaseDto { get; set; } = new ZLJ.Application.Common.Share.User.UserCreateDto();

}