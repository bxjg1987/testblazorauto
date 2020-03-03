using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo;

namespace ZLJ.Organizations.Dto
{
    /// <summary>
    /// 更新模型
    /// </summary>
    //[AutoMapTo(typeof(OrganizationUnitEntity))]
    public class EditOrganizationUnitDto : GeneralTreeNodeEditBaseDto
    {
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        [Range(0, 2)]
        public int OUType { get; set; }

    }
}
