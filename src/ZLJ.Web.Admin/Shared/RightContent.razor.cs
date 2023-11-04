using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;

namespace ZLJ.Web.Admin.Shared
{
    public partial class RightContent
    {
        private NoticeIconData[] _notifications = { 
            new NoticeIconData {  Avatar="x", Datetime=DateTime.Now, Description="xxx", Extra="xx", Key="xxcv", Read=false, Title="xxxxxxxxxxxx"}
        };
        private NoticeIconData[] _messages = { 
        new NoticeIconData{ Avatar="yyy" , Datetime=DateTime.Now, Description="yyyyyyy  ", Extra="x",
         Key="sdfdf4", Read=true, Style="", Title="yyyyyyyyyyyyy"}
        };
        private NoticeIconData[] _events = {
          new NoticeIconData{ Avatar="yyy" , Datetime=DateTime.Now, Description="yyyyyyy  ", Extra="x",
         Key="sdfd44f4", Read=true, Style="", Title="yyyyyyyyyyyyy"}
        };
        private int _count = 40;

        private List<AutoCompleteDataItem<string>> DefaultOptions { get; set; } = new List<AutoCompleteDataItem<string>>
        {
            new AutoCompleteDataItem<string>
            {
                Label = "umi ui",
                Value = "umi ui"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Table",
                Value = "Pro Table"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Layout",
                Value = "Pro Layout"
            }
        };

        public AvatarMenuItem[] AvatarMenuItems { get; set; } = new AvatarMenuItem[]
        {
            new() { Key = "center", IconType = "user", Option = "个人中心"},
            new() { Key = "setting", IconType = "setting", Option = "个人设置"},
            new() { IsDivider = true },
            new() { Key = "logout", IconType = "logout", Option = "退出登录"}
        };

        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Inject] protected MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Clear()
                .Add("right");
        }

        public void HandleSelectUser(MenuItem item)
        {
            switch (item.Key)
            {
                case "center":
                    NavigationManager.NavigateTo("/account/center");
                    break;
                case "setting":
                    NavigationManager.NavigateTo("/account/settings");
                    break;
                case "logout":
                    NavigationManager.NavigateTo("/user/login");
                    break;
            }
        }

        public void HandleSelectLang(MenuItem item)
        {
        }

        public async Task HandleClear(string key)
        {
            switch (key)
            {
                case "notification":
                    _notifications = new NoticeIconData[] { };
                    break;
                case "message":
                    _messages = new NoticeIconData[] { };
                    break;
                case "event":
                    _events = new NoticeIconData[] { };
                    break;
            }
            await MessageService.Success($"清空了{key}");
        }

        public async Task HandleViewMore(string key)
        {
            await MessageService.Info("Click on view more");
        }
    }
}