using Abp.Application.Services.Dto;


namespace BXJG.Utils.Application.Share.Session
{
    // [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
