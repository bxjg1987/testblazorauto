using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 列表页搜索框、表单页下拉框查询使用的输入模型
    /// </summary>
    public class GeneralTreeGetForSelectInput //: GetForSelectInput
    {
        /// <summary>
        /// 0直接返回查找到的后代节点，
        /// 
        /// 大于0的都会尝试增加父节点的返回
        /// 若ParentText不为空，则取ParentText的本地化文本后创建一个虚拟节点作为父节点，子节点为查询到的列表
        /// 1若ParentText为空，则尝试加载Id对应的父节点，若未找到则通过Search创建虚拟节点
        /// 2若未找到则通过Search创建虚拟节点
        ///   
        /// 3若ParentText为空，则尝试加载Id对应的父节点，若未找到则通过Form创建虚拟节点
        /// 4若未找到则通过Form创建虚拟节点
        /// </summary>
        //public int ForType { get; set; }
        /// <summary>
        /// 给前端一个机会来设置自己想要的值，注意是否需要本地化调用方来决定
        /// </summary>
        // public string ParentText { get; set; }

        //2022-5-3 变形精怪 增加
        //通用树模块目前还未使用此字段，但应该添加此字段，它比ParentId会少一次查询

        /// <summary>
        /// code可能会变，所以不要硬编码时用code
        /// </summary>
        //[Obsolete("code可能会变，所以不要硬编码时用code")]
        public string? Code { get; set; }
        public long? ParentId { get; set; }
        /// <summary>
        /// 是否仅仅加载子节点，true只加载子节点，false加载所有后台节点
        /// </summary>
        public bool IsOnlyLoadChild { get; set; } = false;
    }
}