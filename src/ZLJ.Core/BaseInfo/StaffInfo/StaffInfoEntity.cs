using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.BusinessUser;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Administrative;
using ZLJ.Core.BaseInfo.Post;
using BXJG.Common.Extensions;
using Abp.Timing;
using BXJG.Common.Contracts;

namespace ZLJ.Core.BaseInfo.StaffInfo
{
    /// <summary>
    /// 员工档案实体类
    /// </summary>
    public class StaffInfoEntity : User
    {
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        private DateTimeOffset? sr;
        /// <summary>
        /// 出生年月日
        /// </summary>
        public DateTimeOffset? Birthday
        {
            get
            {
                return sr;
            }
            set
            {
                sr = value;
                if (sr.HasValue)
                {
                    var r = sr.Value.DateTime.CalculateAge(Clock.Now);
                    AgeYears = r.years;
                    AgeMonths = r.months;
                    AgeDays = r.days;
                }
            }
        }
        public int? AgeYears { get; private set; }
        public int? AgeMonths { get; private set; }
        public int? AgeDays { get; private set; }
        //public string AgeString { get; private set; }
        /// <summary>
        /// 工号，这个前期要求用户手动录入，后期考虑自动生成（注意考虑分布式情况）
        /// 唯一索引
        /// </summary>
        public string No { get; set; }
        ///// <summary>
        ///// 手机号
        ///// </summary>
        //public string PhoneNumber { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域实体
        /// </summary>
        public virtual AdministrativeEntity Area { get; set; }
        /// <summary>
        /// 现住地址
        /// </summary>
        public string CurrentAddress { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTimeOffset? InDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTimeOffset? OutDate { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdNumber { get; set; }
    }
}