using DocumentFormat.OpenXml.Drawing.Charts;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Host.Notify
{
    public partial class NotifyManager
    {
        ICollection<object> yixuanze => currDefine == default ? new HashSet<object>() : new HashSet<object> { currDefine.Name };

        ICollection<object> duquZhuangtais => condition.UserNotificationState == default ? new HashSet<object>() : new HashSet<object> { condition.UserNotificationState };

        //private async Task DuquZhuangtaiBianhua(ICollection<object> zts) 
        //{ 
        //    if (zts == default || !zts.Any())
        //        return;

        //    await base.ReadStateChanged(zts.SingleOrDefault());
        //}



        [Inject]
        public IDialogService DialogService { get; set; }
        private async Task NamesChanged(ICollection<object> objects)
        {
            if (objects == default || !objects.Any())
                return;

            await base.HeadChanged(objects.Single().ToString());
        }

        private void OpenDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, CloseButton = true };
            DialogService.Show<NotifySubscript>("通知订阅", options);
        }

        private async Task ClearCondition()
        {
            condition.Keywords = default;
            condition.UserNotificationState = Abp.Notifications.UserNotificationState.Unread;
            //duquZhuangtais = new HashSet<object> { condition.UserNotificationState };
            condition.StartTime = default;
            condition.EndTime = default;
             
            await ConditionChanged();
        }

        private async Task Fanye(int pageIndex) { 
        condition.SkipCount = (pageIndex - 1) * condition.MaxResultCount;
            await LoadMessagesAsync();
        }
    }
}
