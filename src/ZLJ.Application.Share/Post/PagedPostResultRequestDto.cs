using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;

using System.ComponentModel;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Application.Share.Post
{
    public class PagedPostResultRequestDto : PostCondition, IHaveKeywords
    {
        [DisplayName("所属部门")]
        public string? OuCode { get; set; }
        public string? Permission { get; set; }

        //public bool? IsStatic { get; set; }

        //一般的条件都用不需要定义条件了，然动态条件来吧
        //public PostCondition? Conditions { get; set; }= new PostCondition();
        //public string Keywords { get; set; }
        //  public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

