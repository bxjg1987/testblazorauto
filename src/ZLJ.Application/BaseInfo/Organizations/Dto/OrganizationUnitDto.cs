using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo;

namespace ZLJ.Organizations.Dto
{
    //属性首字母要小写，因为编辑时首先获取数据是小写，然后调用easyui的load，若表单name是大写这无法赋值
    /// <summary>
    /// 查询组织单位树形列表所使用的dto模型
    /// </summary>
    //[AutoMapFrom(typeof(OrganizationUnitEntity))]
    public class OrganizationUnitDto : GeneralTreeGetTreeNodeBaseDto<OrganizationUnitDto>
    {
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        public int OUType { get; set; }
        public string OUTypeText { get; set; }
    }
}
