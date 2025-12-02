using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Warehouse
{
    /// <summary>
    /// 仓库分页查询请求
    /// </summary>
    public class WarehousePagedResultRequestDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public WarehouseCondition Condition { get; set; }
        
        public WarehousePagedResultRequestDto()
        {
            Condition = new WarehouseCondition();
        }
    }
}
