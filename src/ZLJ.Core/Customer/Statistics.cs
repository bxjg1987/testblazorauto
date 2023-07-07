using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.AssociatedCompany;

namespace ZLJ.Customer
{
    public abstract class PaperUserReportBaseEntity<TKey> : Entity<long>, IMustHaveCustomer
    {
        ///// <summary>
        ///// 记录上次统计到哪里来了
        ///// </summary>
        //public Guid OrderIndex { get; set; }
        public TKey ClsId { get; set; }
        public string ClsName { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public int Month { get; set; }
        public int A3BlackAndWhiteUse { get; set; }
        public int A3TrueColorUse { get; set; }
        public int A4BlackAndWhiteUse { get; set; }
        public int A4TrueColorUse { get; set; }
        public int A3Use { get; set; }
        public int A4Use { get; set; }
        public int TrueColorUse { get; set; }
        public int BlackWhiteUse { get; set; }
        public int Use { get; set; }
    }

    /// <summary>
    /// 客户那边看的，根据部门统计纸张用量
    /// </summary>
    public class PaperUseReportOUEntity : PaperUserReportBaseEntity<long>
    {
    }

    /// <summary>
    /// 客户那边看的，根据设备统计纸张用量
    /// </summary>
    public class PaperUseReportEquipmentInstanceEntity : PaperUserReportBaseEntity<string>
    {
    }
}