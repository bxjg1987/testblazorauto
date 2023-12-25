using Abp.Application.Services.Dto;

namespace ZLJ.BaseInfo.AssociatedCompany.Dto
{
    public class AssociatedCompanyGetForSelectOutputDto : ComboboxItemDto
    {
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LinkPhone { get; set; }
    }
}