using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ZLJ.Application.Common.Share.Roles;

namespace ZLJ.Application.Share.Post
{
    public class PostCreateDto : PostEditDto
    {
        public RoleCreateDto Dto { get; set; } = new RoleCreateDto();
    }
}
