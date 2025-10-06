using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;

namespace ZLJ.Application.Share.AssociatedCompany
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociatedCompanyDto : FullAuditedEntityDto<long>, IExtendableObj
    {
        [DisplayName("是否启用")]
        public bool IsActive { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("客户名称")]
        public string Name { get; set; }
        ///// <summary>
        ///// 拼音首字母
        ///// </summary>
        //public string Pinyin { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        [DisplayName("税号")]
        public string TaxNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DisplayName("联系人")]
        public string LinkMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        public string LinkPhone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DisplayName("地址")]
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
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }

        /// <summary>
        /// 所属区域名
        /// </summary>
        [DisplayName("所属区域")]
        public string AreaDisplayName { get; set; }

        /// <summary>
        /// 客户等级Id
        /// </summary>
        public long? LevelId { get; set; }

        /// <summary>
        /// 客户等级名称
        /// </summary>
        [DisplayName("客户等级")]
        public string LevelDisplayName { get; set; }
        public dynamic ExtensionData { get; set; }

        ///// <summary>
        ///// 客户类别Id
        ///// </summary>
        //public long? CategoryId { get; set; }

        ///// <summary>
        ///// 客户类别名称
        ///// </summary>
        //public string CategoryDisplayName { get; set; }
        ///// <summary>
        ///// 管理员账号
        ///// </summary>
        //public string AdminUserName { get; set; }
        ///// <summary>
        ///// 管理员密码（新增时返回）
        ///// </summary>
        //public string AdminEquipmentPwd { get; set; }
    }
}