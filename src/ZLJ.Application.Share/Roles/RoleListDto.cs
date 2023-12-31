using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Share.Roles
{
    public class RoleListDto : EntityDto, IHasCreationTime
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreationTime { get; set; }

        public List<OuDto> Ous;
    }

}
