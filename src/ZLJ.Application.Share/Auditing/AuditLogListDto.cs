using Abp.Application.Services.Dto;
using System;

namespace ZLJ.Application.Share.Auditing
{
   
   // [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogListDto : EntityDto<long>
    {
        public long? UserId { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }

        public int? ImpersonatorTenantId { get; set; }

        public long? ImpersonatorUserId { get; set; }
        [DisplayName("服务")]
        public string ServiceName { get; set; }
        [DisplayName("接口")]
        public string MethodName { get; set; }
        [DisplayName("参数")]
        public string Parameters { get; set; }
        //[DisableDateTimeNormalization]
        //[JsonConverter(typeof(ChinaDateTimeConverter))]
        [DisplayName("执行时间")]
        public DateTime ExecutionTime { get; set; }
        [DisplayName("耗时(毫秒)")]
        public int ExecutionDuration { get; set; }
        [DisplayName("客户端ip地址")]
        public string ClientIpAddress { get; set; }
        [DisplayName("客户端名称")]
        public string ClientName { get; set; }
        [DisplayName("浏览器信息")]
        public string BrowserInfo { get; set; }
        [DisplayName("异常信息")]
        public string Exception { get; set; }

        public string CustomData { get; set; }
    }
}