using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Warehouse
{
    /// <summary>
    /// 仓库查询条件
    /// </summary>
    public class WarehouseCondition : IHaveKeywords
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keywords { get; set; }
        
        /// <summary>
        /// 是否是虚拟仓库
        /// </summary>
        public bool? IsVirtual { get; set; }
        
        /// <summary>
        /// 是否是个人仓库
        /// </summary>
        public bool? IsPersonal { get; set; }
        
        /// <summary>
        /// 所属省市区县ID
        /// </summary>
        public long? AreaId { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
