using Abp.Application.Services.Dto;

namespace ZLJ.Application.BaseInfo.Dto.Machine
{
    public class MachineDTO : FullAuditedEntityDto<long>
    {
        public string Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
     
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
        public string Color { get; set; }
        public string Remark { get; set; }
    }
}
