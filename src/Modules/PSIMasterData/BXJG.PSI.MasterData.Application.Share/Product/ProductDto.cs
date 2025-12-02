using Abp.Application.Services.Dto;
using System;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品DTO
    /// </summary>
    public class ProductDto : FullAuditedEntityDto<string>
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 品牌ID
        /// </summary>
        public long? BrandId { get; set; }
        
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        
        /// <summary>
        /// 商品规格型号
        /// </summary>
        public string Model { get; set; }
        
        /// <summary>
        /// 是否是虚拟产品
        /// </summary>
        public bool IsVirtual { get; set; }
        
        /// <summary>
        /// 商品类别ID
        /// </summary>
        public long? CategoryId { get; set; }
        
        /// <summary>
        /// 商品类别名称
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 所属组织机构ID
        /// </summary>
        public long? OrganizationUnitId { get; set; }
        
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtensionData { get; set; }
    }
}