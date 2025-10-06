using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.Kehu
{
    /// <summary>
    /// 通用的，共选择客户时的数据源显示模型
    /// 也可以作为被其它显示模型引用的模型
    /// </summary>
    public class KehuDto : EntityDto<long>
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [DisplayName("客户")]
        public string Name { get; set; }
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
        /// 所属区域
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        [DisplayName("区域")]
        public string AreaDisplayName { get; set; }
        /// <summary>
        /// 客户级别
        /// </summary>
        public long? LevelId { get; set; }
        /// <summary>
        /// 客户级别名称
        /// </summary>
        [DisplayName("客户级别")]
        public string LevelDisplayName { get; set; }
    }
}
