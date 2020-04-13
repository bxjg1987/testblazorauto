using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 什么是业务正确的订单OrderEntity？
     * 一个订单必须指明是哪个客户购买的，必须包含买的哪些商品，订单价格必须根据业务规则计算得来（商品总额+额外费用-优惠等等）
     * 还有其它很多业务规则限制，最终才能成为一个业务正确的订单
     * 
     * 最简单的办法是为OrderEntity定义一个构造函数，然后要求调用方必须传入必要的参数，然后在构造函数内部根据各种业务规则来初始化这个订单
     * 这种方式有太多问题，各种业务规则会设计到很多组件，按DDD的思路 实体类不应该包含那些组件，如果一个实体与其他实体关联则应该定义领域服务
     * 另外实体也作为EF的实体，有太多限制，比如你不能为了限制业务规则 将某些属性私有化
     * 
     * 如何保证创建一个业务正常的订单OrderEntity？
     */
    public class OrderBuilder : ITransientDependency
    {

    }
}
