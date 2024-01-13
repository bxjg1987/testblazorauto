using Abp.Application.Services.Dto;
namespace ZLJ.Application.Common.Customer
{
    /// <summary>
    /// 公共的 获取客户部门下拉框数据的 查询参数
    /// </summary>
    public class OuGetAllInput //: GetForSelectInput
    {
        [Required]
        public long CustomerId { get; set; }
        //public string Keywords { get; set; }
    }
}
