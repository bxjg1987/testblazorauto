using Abp.Application.Services.Dto;


namespace ZLJ.Application.Common.Share.Session
{
   // [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
