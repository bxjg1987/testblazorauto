using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share.GeneralTree;

namespace BXJG.PSI.MasterData.Application.Share.Warehouse
{
    /// <summary>
    /// 仓库选择DTO
    /// </summary>
    public class WarehouseSelectDto : EntityDto<Guid>
    {
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
        /// 仓库地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
