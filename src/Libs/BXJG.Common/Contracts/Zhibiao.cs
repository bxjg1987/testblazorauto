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
    public record Zhibiao
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
        /// 维度name列表，指向<see cref="ZhibiaoBaozhuang.WeiduLiebiao"/>
        /// 通常与<see cref="WeiduLiebiao"/>二选一
        /// </summary>
        public List<string> WeiduMingchengLiebiao { get; set; }
        /// <summary>
        /// 仅当前值的维度列表
        /// 通常与<see cref="WeiduMingchengLiebiao"/>二选一
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
        /// 使用场景举例：在销量统计的折线图中，x轴为时间，y轴为销量，每个时间刻度都是一个维度，它们都被分到同一个“时间”分组中
        /// </summary>
        public string? Fenzu { get; set; }
    }
    /// <summary>
    /// ZhibiaoLiebiao中的指标不再直接包含维度，而是通过指标名称指向WeiduLiebiao
    /// 这样多个指标可以共享一个维度
    /// </summary>
    public record ZhibiaoBaozhuang
    {
        /// <summary>
        /// 额外的数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }


        /// <summary>
        /// <see cref="ZhibiaoLiebiao"/>中的指标通过名称列表指向这里
        /// </summary>
        public List<Weidu> WeiduLiebiao { get; set; } = new();
        /// <summary>
        /// 指标列表
        /// </summary>
        public List<Zhibiao> ZhibiaoLiebiao { get; set; } = new();

    }

    /// <summary>
    /// 指标提示级别
    /// </summary>
    [Flags]
    public enum ZhibiaoTishiJibie
    {
        Moren = 0,
        Home = 1 << 0,
        Cheng = 1 << 1,
        Huang = 1 << 2,
        Lan = 1 << 3,
        Lv = 1 << 4,

    }
}
