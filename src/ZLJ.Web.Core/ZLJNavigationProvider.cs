using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Authorization;
using BXJG.WorkOrder;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;

namespace ZLJ.Navigation
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public partial class ZLJNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            //{codegenerator}

            #region 仓库

            var wms = new MenuItemDefinition(PermissionNames.BXJGWMS,
           PermissionNames.BXJGWMS.GetLocalizableString(),
           icon: "cangku",
           permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMS));
            context.Manager.MainMenu.AddItem(wms);

            //库存查询
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInventory,
                displayName: PermissionNames.BXJGWMSStockInventory.GetLocalizableString(),
                icon: "kucunchaxun",
                url: $"/{PermissionNames.BXJGWMS}/stockinventory/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInventory)));

            //库存统计
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInventoryStatistics,
                displayName: PermissionNames.BXJGWMSStockInventoryStatistics.GetLocalizableString(),
                icon: "tongji",
                url: $"/{PermissionNames.BXJGWMS}/stockinventorystatistics/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInventoryStatistics)));

            #region 入库管理
            ////入库管理
            //wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockIn,
            //    displayName: PermissionNames.BXJGWMSStockIn.GetLocalizableString(),
            //    icon: "ruku",
            //    url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockIn)));

            var stockin = new MenuItemDefinition(PermissionNames.BXJGWMSStockIn,
          PermissionNames.BXJGWMSStockIn.GetLocalizableString(),
          icon: "cangku",
          permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockIn));
            wms.AddItem(stockin);

            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInBuy,
                displayName: PermissionNames.BXJGWMSStockInBuy.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInBuy)));

            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInReturnGood,
                displayName: PermissionNames.BXJGWMSStockInReturnGood.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInReturnGood)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInReturnRent,
                displayName: PermissionNames.BXJGWMSStockInReturnRent.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInReturnRent)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInEarn,
                displayName: PermissionNames.BXJGWMSStockInEarn.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInEarn)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInGroup,
                displayName: PermissionNames.BXJGWMSStockInGroup.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInGroup)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInPull,
                displayName: PermissionNames.BXJGWMSStockInPull.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInPull)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInBack,
                displayName: PermissionNames.BXJGWMSStockInBack.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInBack)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInTransfer,
                displayName: PermissionNames.BXJGWMSStockInTransfer.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInTransfer)));
            //入库管理
            stockin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockInOther,
                displayName: PermissionNames.BXJGWMSStockInOther.GetLocalizableString(),
                icon: "ruku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockInOther)));

            #endregion

            #region 出库管理
            ////出库管理
            //wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOut,
            //    displayName: PermissionNames.BXJGWMSStockOut.GetLocalizableString(),
            //    icon: "chuku",
            //    url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOut)));

            var stockout = new MenuItemDefinition(PermissionNames.BXJGWMSStockOut,
          PermissionNames.BXJGWMSStockOut.GetLocalizableString(),
          icon: "cangku",
          permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOut));
            wms.AddItem(stockout);


            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutSale,
                displayName: PermissionNames.BXJGWMSStockOutSale.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutSale)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutBuyReturn,
                displayName: PermissionNames.BXJGWMSStockOutBuyReturn.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutBuyReturn)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutRent,
                displayName: PermissionNames.BXJGWMSStockOutRent.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutRent)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutLose,
                displayName: PermissionNames.BXJGWMSStockOutLose.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutLose)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutGroup,
                displayName: PermissionNames.BXJGWMSStockOutGroup.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutGroup)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutDismantle,
                displayName: PermissionNames.BXJGWMSStockOutDismantle.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutDismantle)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutLend,
                displayName: PermissionNames.BXJGWMSStockOutLend.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutLend)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutLoss,
                displayName: PermissionNames.BXJGWMSStockOutLoss.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutLoss)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutTransfer,
                displayName: PermissionNames.BXJGWMSStockOutTransfer.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutTransfer)));
            //出库管理
            stockout.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockOutOther,
                displayName: PermissionNames.BXJGWMSStockOutOther.GetLocalizableString(),
                icon: "chuku",
                url: $"/{PermissionNames.BXJGWMS}/stockout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockOutOther)));
            #endregion


            //库存盘点
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockCheck,
                displayName: PermissionNames.BXJGWMSStockCheck.GetLocalizableString(),
                icon: "pandian",
                url: $"/{PermissionNames.BXJGWMS}/stockcheck/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockCheck)));

            //库存调拨
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockTransfer,
                displayName: PermissionNames.BXJGWMSStockTransfer.GetLocalizableString(),
                icon: "diaobo",
                url: $"/{PermissionNames.BXJGWMS}/transfer/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockTransfer)));

            //拆机
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockDismantle,
                displayName: PermissionNames.BXJGWMSStockDismantle.GetLocalizableString(),
                icon: "chaizhuang",
                url: $"/{PermissionNames.BXJGWMS}/dismantle/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockDismantle)));

            //装机
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockMake,
                displayName: PermissionNames.BXJGWMSStockMake.GetLocalizableString(),
                icon: "chaizhuang",
                url: $"/{PermissionNames.BXJGWMS}/make/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockMake)));

            //仓库信息
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSBaseStore,
                displayName: PermissionNames.BXJGWMSBaseStore.GetLocalizableString(),
                icon: "cangku",
                url: $"/{PermissionNames.BXJGWMS}/basestore/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSBaseStore)));
            //库存预警限值
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockLimit,
                displayName: PermissionNames.BXJGWMSStockLimit.GetLocalizableString(),
                icon: "cangku",
                url: $"/{PermissionNames.BXJGWMS}/stocklimit/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockLimit)));
            #endregion


            #region 工单

            //var topMenu = context.Manager.MainMenu.add
            var topMenu = new MenuItemDefinition(PermissionNames.WorkOrder,
                PermissionNames.WorkOrder.GetLocalizableString(),
                icon: PermissionNames.WorkOrderManager,
                permissionDependency: new SimplePermissionDependency(PermissionNames.WorkOrder));
            context.Manager.MainMenu.AddItem(topMenu);
            topMenu.AddBXJGWorkOrderCategoryNav();

            topMenu.AddItem(
                new MenuItemDefinition(
                    name: PermissionNames.WorkOrderManager,
                    displayName: PermissionNames.WorkOrderManager.GetLocalizableString(),
                    icon: PermissionNames.WorkOrderManager,
                    url: $"/{BXJG.WorkOrder.CoreConsts.WorkOrder}/RentOrderItemWorkOrder/index.html",
                    requiresAuthentication: true,
                    permissionDependency: new SimplePermissionDependency(PermissionNames.WorkOrderManager)
                )
            );

            topMenu.AddItem(
                new MenuItemDefinition(
                    name: PermissionNames.WorkOrderConfig,
                    displayName: PermissionNames.WorkOrderConfig.GetLocalizableString(),
                    icon: PermissionNames.WorkOrderConfig,
                    url: $"/{BXJG.WorkOrder.CoreConsts.WorkOrder}/WorkOrderConfig/index.html",
                    requiresAuthentication: true,
                    permissionDependency: new SimplePermissionDependency(PermissionNames.WorkOrderConfig)
                )
            );
            topMenu.AddItem(
                new MenuItemDefinition(
                    name: PermissionNames.BXJGBaseInfoWorkloadRuleDefinition,
                    displayName: PermissionNames.BXJGBaseInfoWorkloadRuleDefinition.GetLocalizableString(),
                    icon: PermissionNames.BXJGBaseInfoWorkloadRuleDefinition,
                    url: $"/{BXJG.WorkOrder.CoreConsts.WorkOrder}/WorkloadRule/index.html",
                    requiresAuthentication: true,
                    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoWorkloadRuleDefinition)
                )
            );
            //context.Manager.MainMenu.AddBXJGWorkOrderAllNav();

            #endregion

            #region 设备管理

            SetEquipmentNavigation(context);

            #endregion

            #region 租赁

            var zulin = new MenuItemDefinition(PermissionNames.BXJGRent,
                PermissionNames.BXJGRent.GetLocalizableString(),
                icon: PermissionNames.BXJGRent,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRent));
            context.Manager.MainMenu.AddItem(zulin);

            //租赁订单
            zulin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGRentOrder,
                displayName: PermissionNames.BXJGRentOrder.GetLocalizableString(),
                icon: "WorkOrderManager",
                url: $"/{PermissionNames.BXJGRent}/order/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRentOrder)));

            //抄表
            zulin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGRentSettle,
                displayName: PermissionNames.BXJGRentSettle.GetLocalizableString(),
                icon: "zhangdan",
                url: $"/{PermissionNames.BXJGRent}/settle/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRentSettle)));

            ////退租单
            //zulin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGRentRedelivery,
            //    displayName: PermissionNames.BXJGRentRedelivery.GetLocalizableString(),
            //    icon: "chuku",
            //    url: $"/{PermissionNames.BXJGRent}/redelivery/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRentRedelivery)));

            //租赁套餐
            zulin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGRentSetMeal,
                displayName: PermissionNames.BXJGRentSetMeal.GetLocalizableString(),
                icon: "taocan",
                url: $"/{PermissionNames.BXJGRent}/setmeal/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRentSetMeal)));

            //合同模板
            zulin.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGRentContractTemplate,
                displayName: PermissionNames.BXJGRentContractTemplate.GetLocalizableString(),
                icon: "moban",
                url: $"/{PermissionNames.BXJGRent}/contractTemplate/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGRentContractTemplate)));
            #endregion

            #region 采购管理
            var purchase = new MenuItemDefinition(PermissionNames.BXJGPurchase,
           PermissionNames.BXJGPurchase.GetLocalizableString(),
           icon: "purchase",
           permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPurchase));
            //context.Manager.MainMenu.AddItem(purchase);
            wms.AddItem(purchase);

            //采购订单
            purchase.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPurchaseOrder,
                displayName: PermissionNames.BXJGPurchaseOrder.GetLocalizableString(),
                icon: "purchase",
                url: $"/{PermissionNames.BXJGPurchase}/purchaseorder/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPurchaseOrder)));


            //采购退货单
            purchase.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPurchaseReturnOrder,
                displayName: PermissionNames.BXJGPurchaseReturnOrder.GetLocalizableString(),
                icon: "purchase-return",
                url: $"/{PermissionNames.BXJGPurchase}/purchasereturnorder/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPurchaseReturnOrder)));
            #endregion

            #region 销售管理
            var sale = new MenuItemDefinition(PermissionNames.BXJGSale,
           PermissionNames.BXJGSale.GetLocalizableString(),
           icon: "sale",
           permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGSale));
            wms.AddItem(sale);
            //context.Manager.MainMenu.AddItem(sale);

            //销售订单
            sale.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGSaleOrder,
                displayName: PermissionNames.BXJGSaleOrder.GetLocalizableString(),
                icon: "sale",
                url: $"/{PermissionNames.BXJGSale}/saleorder/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGSaleOrder)));


            //销售退货单
            sale.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGSaleReturnOrder,
                displayName: PermissionNames.BXJGSaleReturnOrder.GetLocalizableString(),
                icon: "sale-return",
                url: $"/{PermissionNames.BXJGSale}/salereturnorder/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGSaleReturnOrder)));
            #endregion

            #region 财务管理
            var pay = new MenuItemDefinition(PermissionNames.BXJGPay,
           PermissionNames.BXJGPay.GetLocalizableString(),
           icon: "pay",
           permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPay));
            context.Manager.MainMenu.AddItem(pay);

            //应付款单
            pay.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPayoutDue,
                displayName: PermissionNames.BXJGPayoutDue.GetLocalizableString(),
                icon: "pay-out",
                url: $"/{PermissionNames.BXJGPay}/payoutdue/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPayoutDue)));

            //付款单
            pay.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPayout,
                displayName: PermissionNames.BXJGPayout.GetLocalizableString(),
                icon: "pay-out",
                url: $"/{PermissionNames.BXJGPay}/payout/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPayout)));

            //应收款单
            pay.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPayinDue,
                displayName: PermissionNames.BXJGPayinDue.GetLocalizableString(),
                icon: "pay-in",
                url: $"/{PermissionNames.BXJGPay}/payindue/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPayinDue)));

            //收款单
            pay.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGPayin,
                displayName: PermissionNames.BXJGPayin.GetLocalizableString(),
                icon: "pay-in",
                url: $"/{PermissionNames.BXJGPay}/payin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGPayin)));

            #endregion

            #region 车辆
            var car = new MenuItemDefinition(PermissionNames.BXJGCar,
           PermissionNames.BXJGCar.GetLocalizableString(),
           icon: "car",
           permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGCar));
            context.Manager.MainMenu.AddItem(car);

            //用车管理
            car.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGCarUse,
                displayName: PermissionNames.BXJGCarUse.GetLocalizableString(),
                icon: "car-use",
                url: $"/{PermissionNames.BXJGCar}/caruse/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGCarUse)));
            #endregion

            #region 基础资料

            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                PermissionNames.AdministratorBaseInfo.GetLocalizableString(),
                icon: "shuju",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            context.Manager.MainMenu.AddItem(menuBaseInfo);

            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
              displayName: PermissionNames.AdministratorBaseInfoOrganizationUnit.GetLocalizableString(),
              icon: "zuzhi",
              url: $"/bxjgbaseinfo/organizationUnit/index.html",
              requiresAuthentication: true,
              permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                PermissionNames.AdministratorBaseInfoPost.GetLocalizableString(),
                icon: "groupbai",
                url: "/bxjgbaseinfo/post/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetLocalizableString(),
                icon: "user",
                url: $"/bxjgbaseinfo/staffInfo/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            //来往单位
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
                displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetLocalizableString(),
                icon: "group",
                url: $"/bxjgbaseinfo/associatedCompany/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));


            //设备档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoEquipmentInfo,
                displayName: PermissionNames.BXJGBaseInfoEquipmentInfo.GetLocalizableString(),
                icon: "dangan",
                url: $"/bxjgbaseinfo/equipmentinfo/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoEquipmentInfo)));


            //设备故障
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoEquipmentFaultDefinition,
            //    displayName: PermissionNames.BXJGBaseInfoEquipmentFaultDefinition.GetLocalizableString(),
            //    icon: "zuzhi",
            //    url: $"/bxjgbaseinfo/equipmentFaultDefinition/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames
            //        .BXJGBaseInfoEquipmentFaultDefinition)));

            var sjzd = menuBaseInfo.AddGeneralTreeNavigation();
            sjzd.Icon = "shuju";
            sjzd.Url = "/bxjgbaseinfo/generalTree/index.html";

            ////整机档案
            //jichuziliao.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSMachineArchives,
            //    displayName: PermissionNames.BXJGWMSMachineArchives.GetLocalizableString(),
            //    icon: "shebei",
            //    url: $"/{PermissionNames.BXJGWMS}/machineArchives/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSMachineArchives)));
            //配件档案
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSFittingArchives,
            //    displayName: PermissionNames.BXJGWMSFittingArchives.GetLocalizableString(),
            //    icon: "gongju",
            //    url: $"/{PermissionNames.BXJGWMS}/fittingArchives/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSFittingArchives)));
            ////耗材档案
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSConsumeArchives,
            //    displayName: PermissionNames.BXJGWMSConsumeArchives.GetLocalizableString(),
            //    icon: "zican",
            //    url: $"/{PermissionNames.BXJGWMS}/consumeArchives/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSConsumeArchives)));

            //行政区域
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
                displayName: PermissionNames.BXJGBaseInfoAdministrative.GetLocalizableString(),
                icon: "qizi",
                url: $"/bxjgbaseinfo/Administrative/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAdministrative)));

            //直接使用用户
            ////员工档案
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
            //    displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetLocalizableString(),
            //    icon: "user",
            //    url: $"/bxjgbaseinfo/staffInfo/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));


            //--codegenerator.BaseInfo==


            //var xtsz = new MenuItemDefinition("System",
            //    L("System"),
            //    icon: "config",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystem));
            //context.Manager.MainMenu.AddItem(xtsz);

            menuBaseInfo.AddItem(new MenuItemDefinition("AdminTenant",
                L("Tenant"),
                icon: "filter",
                url: "/system/tenant/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemTenant)));

            menuBaseInfo.AddItem(new MenuItemDefinition("AdminRole",
                L("Role"),
                icon: "groupbai",
                url: "/system/role/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemRole)));
            menuBaseInfo.AddItem(new MenuItemDefinition("AdminUser",
                L("User"),
                icon: "user",
                url: "/system/user/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemUser)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemLog",
                L("Log"),
                icon: "lishi",
                url: "/system/Auditing.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));
            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                L("Settings"),
                icon: "config",
                url: "/system/settings.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}