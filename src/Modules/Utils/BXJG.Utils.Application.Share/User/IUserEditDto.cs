using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 用户编辑模型接口
    /// </summary>
    public interface IUserEditDto : IEntityDto<long>
    {
        /// <summary>
        /// 所属角色名称列表
        /// </summary>
        string[] RoleNames { get; set; }
        /// <summary>
        /// 所属组织机构id列表
        /// </summary>
        List<long> OrganizationUnits { get; set; }
        /// <summary>
        /// 是否修改密码
        /// </summary>
        bool ChangePassword { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        string? Password { get; set; }
    }
}