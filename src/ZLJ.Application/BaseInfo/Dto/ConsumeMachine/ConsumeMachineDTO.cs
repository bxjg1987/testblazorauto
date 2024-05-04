using Abp.Application.Services.Dto;

namespace ZLJ.Application.BaseInfo.Dto.ConsumeMachine
{
    public class ConsumeMachineDTO : FullAuditedEntityDto<long>
    {
        public long ConsumeId { get; set; }
        public long MachineId { get; set; }
        public string Pattern { get; set; }
    }
}
