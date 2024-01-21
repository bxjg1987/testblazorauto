using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Utils.GeneralTree;
using ZLJ.Core.BaseInfo.Administrative;
using ZLJ.Core.Customer;

namespace ZLJ.Core.BaseInfo.AssociatedCompany
{
    /// <summary>
    /// 来往单位
    /// </summary>
    public class AssociatedCompanyEntity : FullAuditedEntity<long>, IMustHaveTenant, IPassivable
    {
        public string Pinyin { get; set; }
        public bool IsActive { get; set; } = true;
        public int TenantId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LinkPhone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Lat { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域实体
        /// </summary>
        public virtual AdministrativeEntity Area { get; set; }

        /// <summary>
        /// 客户等级Id
        /// 不同客户等级可能需要不同的业务处理，这里用字典不太合适
        /// 暂时保持不变，如果将来用枚举，应使用原来的数据字典的id值作为枚举值
        /// </summary>
        public long? LevelId { get; set; }

        /// <summary>
        /// 客户等级实体
        /// </summary>
        public virtual DataDictionaryEntity Level { get; set; }

        /// <summary>
        /// 客户类别Id
        /// 这里本来是区分供应商？客户?还是两者都是
        /// 应该用枚举，应为业务上可能需要硬编码判断
        /// 目前还是保持原样，没有换成枚举
        /// 将来如果要换成枚举，应与字典值保持一致
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 客户类别实体
        /// </summary>
        public virtual DataDictionaryEntity Category { get; set; }
        /// <summary>
        /// 部门集合
        /// </summary>
        public virtual List<CustomerOUEntity> Ous { get; set; }
        /// <summary>
        /// 员工集合
        /// </summary>
        public virtual List<CustomerStaffInfoEntity> Staffs { get; set; }
        /// <summary>
        /// 管理员id
        /// 由于是共享表，所以允许可空
        /// </summary>
        public long? AdminId { get; set; }
        public virtual CustomerStaffInfoEntity Admin { get; set; }
        ///// <summary>
        ///// 报表任务是否已注册
        ///// 不需要中间状态，大不了多注册一次
        ///// </summary>
        //public bool IsReportWorkerRegistered { get; set; } = false;
    }
}