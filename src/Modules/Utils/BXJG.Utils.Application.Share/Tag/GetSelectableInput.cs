using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Tag
{
    /// <summary>
    /// 获取可选tag列表的输入参数
    /// </summary>
    public class GetSelectableInput
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        [Required]
        public required string EntityType { get; set; }
        /// <summary>
        /// 属性名，可空
        /// 属性名，可空 比如工单：字段A表示要处理的问题相关tag，字段B表示处理完成时拍摄的tag，它们都使用tag表，当通过此字段来表示关联的不同的属性
        /// </summary>
        public string? PropertyName { get; set; }
        /// <summary>
        /// 取热度最高的多少个
        /// </summary>
        public int Top { get; set; } = 20;
        /// <summary>
        /// 是否将tag表中的数据也作为数据源
        /// 为空，则取数据提供器定义中的设置
        /// </summary>
        public bool? LoadFromDb { get; set; }
    }
}
