using Abp.Application.Services.Dto;

namespace ZLJ.Application.BaseInfo.Dto.MachineFitting
{
    public class MachineFittingDTO : FullAuditedEntityDto<long>
    {
        public long MachineId { get; set; }
        public long FittingId { get; set; }
        public string FittingName { get; set; }
        public int Count { get; set; }
    }
}
