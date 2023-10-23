using Abp.Application.Navigation;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.BootstrapServer.Shared
{
    public partial class AdminMainLayout
    {

        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }

        ///// <summary>
        ///// OnInitialized 方法
        ///// </summary>
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();

        //    Menus = GetIconSideMenuItems();
        //}
        //private async Task<bool> OnAuthorizing(string name) 
        //{
        //    return false;
        //}
        protected override async Task OnInitializedAsync()
        {
            if (abpSession.UserId.HasValue)
            {
                var menu = await abpNavManager.GetMenuAsync("adminBlazor", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));

                Menus = menu.Items.Select( NewMethod).ToList();
            }
        }
        [Inject]
        public MessageService MessageService { get; set; }
        private static MenuItem NewMethod(UserMenuItem c)
        {
            var temp = new MenuItem(c.DisplayName, c.Url, c.Icon);
            FillMenu(temp, c.Items);
            if (c.Name == "adminBlazor_home")
                temp.Match = NavLinkMatch.All;
            return temp;
        }

        private static void FillMenu(MenuItem parent, IList<UserMenuItem> items)
        {
            if (items != null)
            {
                parent.Items = items.Select(NewMethod);
            }
        }

        //private static List<MenuItem> GetIconSideMenuItems()
        //{
        //    var menus = new List<MenuItem>
        //    {
        //        new MenuItem() { Text = "返回组件库", Icon = "fa-solid fa-fw fa-home", Url = "https://www.blazor.zone/components",  },
        //        new MenuItem() { Text = "Index", Icon = "fa-solid fa-fw fa-flag", Url = "/" , Match = NavLinkMatch.All},
        //        new MenuItem() { Text = "Counter", Icon = "fa-solid fa-fw fa-check-square", Url = "/counter" },
        //        new MenuItem() { Text = "FetchData", Icon = "fa-solid fa-fw fa-database", Url = "fetchdata" },
        //        new MenuItem() { Text = "Table", Icon = "fa-solid fa-fw fa-table", Url = "table" },
        //        new MenuItem() { Text = "花名册", Icon = "fa-solid fa-fw fa-users", Url = "users" }
        //    };

        //    return menus;
        //}

        //public void Dispose() { 

        //}
    }
}
