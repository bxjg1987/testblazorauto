using System;
using System.Collections.Generic;

namespace ZLJ.Application.Common.Dashboard
{
    /// <summary>
    /// 概览统计数据
    /// </summary>
    public class OverviewDto
    {
        /// <summary>
        /// 员工总数
        /// </summary>
        public int StaffCount { get; set; }

        /// <summary>
        /// 本月新增员工数
        /// </summary>
        public int StaffNewThisMonth { get; set; }

        /// <summary>
        /// 组织机构数量
        /// </summary>
        public int OuCount { get; set; }

        /// <summary>
        /// 岗位数量
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// 来往单位数量
        /// </summary>
        public int AssociatedCompanyCount { get; set; }

        /// <summary>
        /// 本月新增来往单位数
        /// </summary>
        public int AssociatedCompanyNewThisMonth { get; set; }

        /// <summary>
        /// 租户总数（仅Host端有值）
        /// </summary>
        public int? TenantCount { get; set; }

        /// <summary>
        /// 活跃租户数（仅Host端有值）
        /// </summary>
        public int? ActiveTenantCount { get; set; }
    }

    /// <summary>
    /// 操作趋势数据项
    /// </summary>
    public class TrendItemDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public long Count { get; set; }
    }

    /// <summary>
    /// 操作类型分布数据项
    /// </summary>
    public class OperationTypeDto
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public long Count { get; set; }
    }

    /// <summary>
    /// 活跃用户数据项
    /// </summary>
    public class ActiveUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作次数
        /// </summary>
        public long Count { get; set; }
    }

    /// <summary>
    /// 待办事项数据项
    /// </summary>
    public class TodoItemDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        DateTime Time { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool Completed { get; set; }
    }

    /// <summary>
    /// 最近动态数据项
    /// </summary>
    public class RecentActivityDto
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
    }
}
