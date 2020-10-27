using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前端顾客下单时提交的数据
    /// </summary>
    public class CustomerOrderCreateDto
    {
        /// <summary>
        /// 顾客填写的备注
        /// </summary>
        [StringLength(CoreConsts.CustomerRemarkMaxLength)]
        public string CustomerRemark { get; set; }
        /// <summary>
        /// 收货人所属区域Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        [Required]
        [StringLength(CoreConsts.ConsigneeMaxLength)]
        public string Consignee { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        [Required]
        [StringLength(CoreConsts.ConsigneePhoneNumberMaxLength)]
        public string ConsigneePhoneNumber { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        [Required]
        [StringLength(CoreConsts.ReceivingAddressMaxLength)]
        public string ReceivingAddress { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        [Required]
        public List<OrderItemCreateDto> Items { get; set; }
    }
}
