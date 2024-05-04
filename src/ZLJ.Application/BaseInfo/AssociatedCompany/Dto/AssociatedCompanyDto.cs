using Abp.Application.Services.Dto;

namespace ZLJ.Application.BaseInfo.AssociatedCompany.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociatedCompanyDto : FullAuditedEntityDto<long>
    {
        public bool IsActive { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        ///// <summary>
        ///// 拼音首字母
        ///// </summary>
        //public string Pinyin { get; set; }
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
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }

        /// <summary>
        /// 所属区域名
        /// </summary>
        public string AreaDisplayName { get; set; }

        /// <summary>
        /// 客户等级Id
        /// </summary>
        public long? LevelId { get; set; }

        /// <summary>
        /// 客户等级名称
        /// </summary>
        public string LevelDisplayName { get; set; }

        /// <summary>
        /// 客户类别Id
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 客户类别名称
        /// </summary>
        public string CategoryDisplayName { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string AdminUserName { get; set; }
        /// <summary>
        /// 管理员密码（新增时返回）
        /// </summary>
        public string AdminEquipmentPwd { get; set; }
    }
}