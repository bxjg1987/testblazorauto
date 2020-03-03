using System.Collections.Generic;
using ZLJ.Roles.Dto;

namespace ZLJ.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}