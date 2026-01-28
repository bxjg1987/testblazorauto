using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share.Files
{
    /// <summary>
    /// 单独操作某个实体的附件的抽象类
    /// </summary>
    public class OperrationInput<TItem>:EntityDto<string>
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string? PropertyName { get; set; }
        /// <summary>
        /// 文件或文件id
        /// </summary>
        public List<TItem> Items { get; set; }
    }
}
