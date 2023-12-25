using Abp.Application.Services.Dto;

namespace ZLJ.App.Admin.BaseInfo.Dto.Consume
{
    public class ConsumeDTO : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public string Supplier { get; set; }

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
