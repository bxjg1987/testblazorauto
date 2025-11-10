using Abp.Application.Services.Dto;
using System;

namespace BXJG.Utils.Application.Share.User
{
    //custom PagedResultRequestDto
    [Obsolete("最好还是用泛型版本加条件对象")]
    public class UserPagedResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public long? RoleId { get; set; }
    }
}
