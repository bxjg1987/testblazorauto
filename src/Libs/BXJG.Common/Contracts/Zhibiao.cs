using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 一个统计数字
    /// 值 + 维度列表
    /// </summary>
    public  record  Zhibiao
    {
        /// <summary>
        /// 额外的数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; } 


        /// <summary>
        /// 值，如：100
        /// </summary>
        public decimal Zhi { get; set; }
        /// <summary>
        /// 某些场景希望后端统一控制显示值，比如小数位数
        /// </summary>
        public string ZhiStr { get; set; }
        /// <summary>
        /// 单位，如：张
        /// </summary>
        public string Danwei { get; set; }
        /// <summary>
        /// 提示级别，值越小 越需要醒目提示，0标识不需要特别提示
        /// </summary>
        public ZhibiaoTishiJibie TishiJibie { get; set; }
        /// <summary>
        /// 与其他值共享的维度列表
        /// </summary>
        public List<string> WeiduMingchengLiebiao { get; set; }
        /// <summary>
        /// 仅当前值的维度列表
        /// </summary>
        public List<Weidu> WeiduLiebiao { get; set; }

    }
    /// <summary>
    /// 维度
    /// </summary>
    public record Weidu 
    {
        /// <summary>
        /// 额外的数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }


        /// <summary>
        /// 名称，如：hb
        /// </summary>
        public string Mingcheng { get; set; }
        /// <summary>
        /// 显示名称 如：黑白
        /// </summary>
        public string MingchengXianshi { get; set; }
        /// <summary>
        /// 维度分组
        /// </summary>
        public string? Fenzu  { get; set; }
    }

    public record ZhibiaoBaozhuang
    {
        /// <summary>
        /// 额外的数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }


        /// <summary>
        /// 仅当前值的维度列表
        /// </summary>
        public List<Weidu> WeiduLiebiao { get; set; }
        /// <summary>
        /// 指标列表
        /// </summary>
        public List<Zhibiao> ZhibiaoLiebiao { get; set; }

    }

    /// <summary>
    /// 指标提示级别
    /// </summary>
    [Flags]
    public enum ZhibiaoTishiJibie
    { 
        Moren = 0,
        Home = 1<<0,
        Cheng = 1<<1,
        Huang = 1<<2,
        Lan = 1<<3,
        Lv  = 1<<4,
        
    }
}
