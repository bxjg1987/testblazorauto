using Abp.Application.Services.Dto;

namespace ZLJ.App.Common.AssociatedCompany
{
    public class Dto : ComboboxItemDto
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