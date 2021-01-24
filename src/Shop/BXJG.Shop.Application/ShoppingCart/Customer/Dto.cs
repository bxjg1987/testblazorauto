using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart.Customer
{
    /*
     * 顾客端针对购物车的操作相关dto
     */

    /// <summary>
    /// 购物车明细Dto基类
    /// </summary>
    public class ItemBaseDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public long ProductId { get; set; }
        /// <summary>
        /// skuId，如果有的话
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
    }
    /// <summary>
    /// 顾客获取服务端购物车时的输入模型
    /// <rb />需要上传本地购物车，由服务端决定是否合并
    /// </summary>
    public class GetInput
    {
        /// <summary>
        /// 购物车明细
        /// </summary>
        public List<GetItemInput> Items { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public Dictionary<string, object> ExtensionData { get; set; }
    }
    /// <summary>
    /// 顾客获取服务端购物车时的输入模型的购物车明细
    /// </summary>
    public class GetItemInput : ItemBaseDto
    {

    }
    /// <summary>
    /// 顾客获取购物车信息的输出模型
    /// </summary>
    public class GetOutput
    {
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 购物车中包含的商品明细的可得积分总额
        /// </summary>
        public int IntegralTotal { get; private set; }
        /// <summary>
        /// 购物车明细
        /// </summary>
        public List<GetItemOutput> Items { get; set; }
    }
    /// <summary>
    /// 顾客获取购物车信息的输出模型中的购物车明细
    /// </summary>
    public class GetItemOutput : ItemBaseDto
    {
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 购物车中包含的商品明细的可得积分总额
        /// </summary>
        public int IntegralTotal { get; private set; }
    }
    /// <summary>
    /// 顾客将商品加入购物车时的出入模型
    /// </summary>
    public class AddItemInput : ItemBaseDto
    {
        /// <summary>
        /// 顾客将商品加入购物车时的出入模型
        /// </summary>
        public AddItemInput()
        {
            Quantity = 1;
        }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public Dictionary<string, object> ExtensionData { get; set; }
    }
    /// <summary>
    /// 顾客将商品加入购物车时的输出模型
    /// </summary>
    public class AddItemOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    /// <summary>
    /// 调整购物车明细数量的输入模型
    /// </summary>
    public class ChangeItemQuantityInput : EntityDto<long>
    {
        /// <summary>
        /// 在原有数量基础上改变的数量，正数为加、负数为减
        /// </summary>
        public decimal Quantity { get; set; }
    }
    /// <summary>
    /// 调整购物车明细数量的输出模型
    /// </summary>
    public class ChangeItemQuantityOutput
    {

    }
    #region 从购物车中移除明细时的输入/输出模型
    /// <summary>
    /// 从购物车中移除明细时的输入模型
    /// </summary>
    public class RemoveItemInput : BatchOperationInputLong
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    /// <summary>
    /// 从购物车中移除明细时的输出模型
    /// </summary>
    public class RemoveItemOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    #endregion
    #region 清空购物车的输入/输出模型
    /// <summary>
    /// 清空购物车的输入模型
    /// </summary>
    public class ClearInput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    /// <summary>
    /// 清空购物车的输出模型
    /// </summary>
    public class ClearOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    #endregion
}
