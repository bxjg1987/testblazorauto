using Abp.Application.Navigation;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Admin.CoreRCL
{
    public partial class MainLayout
    {

        private MenuDataItem[] _menuData = new MenuDataItem[0];
        [Inject]
        protected IMessageService MessageService { get; set; }
        public LinkItem[] Links { get; set; } =
        {
            new LinkItem
            {
                Key = "Ant Design Blazor",
                Title = "Ant Design Blazor",
                Href = "https://antblazor.com",
                BlankTarget = true,
            },
            new LinkItem
            {
                Key = "github",
                Title = "xxxxx",
                Href = "https://github.com/ant-design-blazor/ant-design-pro-blazor",
                BlankTarget = true,
            },
            new LinkItem
            {
                Key = "Blazor",
                Title = "Blazor",
                Href = "https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor?WT.mc_id=DT-MVP-5003987",
                BlankTarget = true,
            }
        };
       
       // [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {

            if (abpSession.TenantId.HasValue && abpSession.UserId.HasValue)
            {
                var menu = await abpNavManager.GetMenuAsync("adminBlazor", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));
                _menuData = MapMenu(menu.Items);
            }
        }

        MenuDataItem[] MapMenu(IList<UserMenuItem> items, MenuDataItem parent = default)
        {
            var jg = new List<MenuDataItem>();
            foreach (var item in items)
            {
                var tmp = new MenuDataItem
                {
                    Key = item.Name,
                    Path = item.Url,
                    Name = item.DisplayName,
                    Match = NavLinkMatch.Prefix,
                    HideChildrenInMenu = !item.Items.Any(),
                    Icon = item.Icon,
                 
                    //Authority 权限
                    Authority = default,
                    HideInMenu = false,
                    Locale = default,
                    ParentKeys = default
                };
                if (item.Items != default)
                  tmp.  Children = MapMenu(item.Items, tmp);
                jg.Add(tmp);
            }
            var jg2 = jg.ToArray();
            if (parent != default)
                parent.Children = jg2;
            return jg2;
        }



        //  private List<MenuItem>? Menus { get; set; }

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

        //protected override async Task OnInitializedAsync()
        //{
        //    if (abpSession.UserId.HasValue)
        //    {
        //        var menu = await abpNavManager.GetMenuAsync("adminBlazor", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));

        //        Menus = menu.Items.Select(NewMethod).ToList();
        //    }
        //}
        //[Inject]
        //public MessageService MessageService { get; set; }
        //private static MenuItem NewMethod(UserMenuItem c)
        //{
        //    var temp = new MenuItem(c.DisplayName, c.Url, c.Icon);
        //    FillMenu(temp, c.Items);
        //    if (c.Name == "adminBlazor_home")
        //        temp.Match = NavLinkMatch.All;
        //    return temp;
        //}

        //private static void FillMenu(MenuItem parent, IList<UserMenuItem> items)
        //{
        //    if (items != null)
        //    {
        //        parent.Items = items.Select(NewMethod);
        //    }
        //}

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
