using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Application.Share.Post
{
    /// <summary>
    /// 后台管理角色使用的显示模型
    /// </summary>
    public class PostDto : PostCreateDto,IExtendableObj
    {

       public new RoleDto Dto { get; set; } = new RoleDto();
        public dynamic ExtensionData { get => ((IExtendableObj)Dto).ExtensionData; set => ((IExtendableObj)Dto).ExtensionData = value; }
    }

    ///// <summary>
    ///// 不同类型用户关联到用户及其角色时的角色信息
    ///// </summary>
    //public class RoleRelationDto
    //{
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    public int Id { get; set; }
    //    /// <summary>
    //    /// 角色名
    //    /// </summary>
    //    public string Name { get; set; }
    //    /// <summary>
    //    /// 显示名
    //    /// </summary>
    //    public string DisplayName { get; set; }
    //    /// <summary>
    //    /// 角色备注
    //    /// </summary>
    //    public string Description { get; set; }
    //    /// <summary>
    //    /// 是否是静态角色
    //    /// </summary>
    //    public bool IsStatic { get; set; }
    //}
}