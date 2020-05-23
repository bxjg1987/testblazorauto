using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前端查询广告时提供的参数
    /// </summary>
    public class FrontGetAdInput
    {
        /// <summary>
        /// 广告位
        /// </summary>
        [Required]
        public long PositionId { get; set; }
    }
}
