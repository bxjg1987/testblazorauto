using Abp.Domain.Entities;
using Abp.Notifications;
using BXJG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZLJ.Web.Admin.BootstrapServer.Pages
{
    public partial class Main
    {
        [Inject]
        public INotificationPublisher NotificationPublisher { get; set; }
        string btnText;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        void ButtonClick() {
            btnText = DateTime.Now.ToString();
        }
    }
}
