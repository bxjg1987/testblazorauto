namespace ZLJ.Configuration
{
    public static class AppSettingNames
    {
        public const string UiTheme = "App.UiTheme";

        public static class TenantManagement
        {
            public static class Workload
            {
                /// <summary>
                /// 工作量类型(工单量和积分)
                /// </summary>
                public const string WorkloadType = "App.TenantManagement.Workload.WorkloadType";
                /// <summary>
                /// 工作量规则类型
                /// </summary>
                public const string WorkloadRuleType = "App.TenantManagement.Workload.WorkloadRuleType";
                /// <summary>
                /// 默认工作量积分值(未配置工作量规则或者不符合已配置工作量规则 使用此积分值)
                /// </summary>
                public const string WorkloadPoints = "App.TenantManagement.Workload.WorkloadPoints";
            }
            
        }
    }
}
