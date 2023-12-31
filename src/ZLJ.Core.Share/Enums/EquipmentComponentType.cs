using BXJG.Utils.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.Share
{
    [Flags]
    public enum EquipmentComponentType
    {
        #region 碳粉盒
        [Description("青色碳粉"), Color("#0EA9E9")]
        TF_Cyan = 1,
        [Description("红色碳粉"), Color("#EA0A87")]
        TF_Red,
        [Description("黄色碳粉"), Color("#F4CC48")]
        TF_Yellow,
        [Description("黑色碳粉"), Color("#323136")]
        TF_Black,
        #endregion

        #region 鼓组件
        [Description("青色鼓"), Color("#0EA9E9")]
        G_Cyan,
        [Description("红色鼓"), Color("#EA0A87")]
        G_Red,
        [Description("黄色鼓"), Color("#F4CC48")]
        G_Yellow,
        [Description("黑色鼓"), Color("#323136")]
        G_Black,
        #endregion

        #region 显影组件
        [Description("青色载体"), Color("#0EA9E9")]
        XY_Cyan,
        [Description("红色载体"), Color("#EA0A87")]
        XY_Red,
        [Description("黄色载体"), Color("#F4CC48")]
        XY_Yellow,
        [Description("黑色载体"), Color("#323136")]
        XY_Black,
        #endregion

        #region 鼓刮板
        [Description("青色刮板"), Color("#0EA9E9")]
        GB_Cyan ,
        [Description("红色刮板"), Color("#EA0A87")]
        GB_Red,
        [Description("黄色刮板"), Color("#F4CC48")]
        GB_Yellow,
        [Description("黑色刮板"), Color("#323136")]
        GB_Black,
        #endregion

        #region 定影组件
        [Description("上辊"), Color("#5470C6")]
        RollUp,
        [Description("下辊"), Color("#9A60B4")]
        RollDown,
        [Description("上清洁纸"), Color("#88CAE7")]
        CleanPaperUp,
        [Description("下清洁纸 "), Color("#91CC75")]
        CleanPaperDown,
        #endregion

        #region 转印组件
        [Description("转印刮板"), Color("#5470C6")]
        ZYGB,
        [Description("转印1"), Color("#9A60B4")]
        ZY1,
        [Description("转印2"), Color("#88CAE7")]
        ZY2,
        #endregion

        #region 废碳粉盒
        [Description("废粉"), Color("#5470C6")]
        FF,
        #endregion
    }
}