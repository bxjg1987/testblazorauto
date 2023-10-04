using Abp.Domain.Entities;
using Abp.Notifications;
using BXJG.Common;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZLJ.Web.Admin
{
    public partial class Main
    {
        [Inject]
        public INotificationPublisher NotificationPublisher { get; set; }

        [Inject]
        IDialogService DialogService { get; set; }
        //protected override async Task OnInitialized2Async()
        //{
        //await  this.NotificationPublisher.PublishAsync(ZLJ.App.Common.Consts.ENEquipmentInstanceLastAlarmChanged,
        //    new MessageNotificationData($"设备{DateTime.Now.ToLongTimeString()}报警信息发送了变化，"), severity: NotificationSeverity.Error);  
        //}
        MudButton[] buttons= new MudButton[2];
        private async Task suojitongzhi()
        {
         //   MudTreeViewItem<object> sdf;
           // sdf.click
            //var uow = base.UnitOfWorkManager.Begin();
            //await NotificationPublisher.PublishAsync(Consts.ENEquipmentInstanceLockStatusChanged,
            //                                                    new MessageNotificationData($"测试锁机通知...{RandomHelper.GetRandomString(12)}"),
            //                                                    //new EntityIdentifier(typeof(EquipmentInstanceEntity), RandomHelper.GetRandomString(6)),
            //                                                     severity: NotificationSeverity.Info);
            //await uow.CompleteAsync();
        }

        private async Task suojitongzhi2()
        {
            //var uow = base.UnitOfWorkManager.Begin();
            //await NotificationPublisher.PublishAsync(Consts.ENEquipmentInstanceLockStatusChanged,
            //                                                    new MessageNotificationData($"测试锁机通知...{RandomHelper.GetRandomString(12)}"),
            //                                                    new EntityIdentifier(typeof(EquipmentInstanceEntity), "2507359600"),
            //                                                     NotificationSeverity.Info);
            //await uow.CompleteAsync();
        }

        //private async void baojingtongzhi2()
        //{
        //    var dr = DialogService.Show<AbpMudCreateDialog>();
        //    var r = await dr.Result;
        //}

        private async Task baojingtongzhi()
        {
            //var uow = base.UnitOfWorkManager.Begin();
            //await NotificationPublisher.PublishAsync(Consts.ENEquipmentInstanceLastAlarmChanged,
            //                                                    new MessageNotificationData($"测试报警通知...{RandomHelper.GetRandomString(12)}"),
            //                                                    //new EntityIdentifier(typeof(EquipmentInstanceEntity), RandomHelper.GetRandomString(6)),
            //                                                    severity: NotificationSeverity.Error);
            //await uow.CompleteAsync();
        }
    }
}
