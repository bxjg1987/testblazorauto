using Abp.Application.Services.Dto;
using System;

namespace BXJG.PSI.MasterData.Application.Share.Warehouse
{
    /// <summary>
    /// 仓库DTO
    /// </summary>
    public class WarehouseDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }
        
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 是否是虚拟仓库
        /// </summary>
        public bool IsVirtual { get; set; }
        
        /// <summary>
        /// 是否是个人仓库
        /// </summary>
        public bool IsPersonal { get; set; }
        
        /// <summary>
        /// 所属省市区县ID
        /// </summary>
        public long? AreaId { get; set; }
        
        /// <summary>
        /// 省市区县名称
        /// </summary>
        public string AreaName { get; set; }
        
        /// <summary>
        /// 仓库地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 面积，㎡
        /// </summary>
        public int SquareMeasure { get; set; }
        
        /// <summary>
        /// 体积 m³
        /// </summary>
        public int Volume { get; set; }
        
        /// <summary>
        /// 仓库类型
        /// </summary>
        public long WarehouseType { get; set; }
        
        /// <summary>
        /// 负责人ID
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// 纬度，用于地理位置定位
        /// </summary>
        public decimal? Latitude { get; set; }
        
        /// <summary>
        /// 经度，用于地理位置定位
        /// </summary>
        public decimal? Longitude { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        
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
