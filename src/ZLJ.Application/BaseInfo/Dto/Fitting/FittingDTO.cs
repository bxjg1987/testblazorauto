using Abp.Application.Services.Dto;
using ZLJ.Core.Share;
using ZLJ.Core.Localization;

namespace ZLJ.Application.BaseInfo.Dto.Fitting
{
    public class FittingDTO : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 类别
        /// </summary>
        public FittingCategory Category { get; set; }

        public string CategoryName => Category.Enum();
        public string Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        public string Remark { get; set; }
    }
}
