using Abp.Application.Services.Dto;
using System.ComponentModel;
using ZLJ.App.Admin.Roles.Dto;

namespace ZLJ.App.Admin.Post.Dto
{
    public class PagedPostResultRequestDto : PagedRoleResultRequestDto, IHaveKeywords
    {
        [DisplayName("所属部门")]
        public string OuCode { get; set; }
        public string Permission { get; set; }

        public bool? IsStatic { get; set; }

        //一般的条件都用不需要定义条件了，然动态条件来吧

        //public string Keywords { get; set; }
        //  public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

