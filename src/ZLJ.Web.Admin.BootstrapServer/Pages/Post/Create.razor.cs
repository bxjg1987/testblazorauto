using BootstrapBlazor.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.Post.Dto;

namespace ZLJ.Web.Admin.BootstrapServer.Pages.Post
{
    public partial class Create
    {
        [Parameter]
        public PostDto CreatedDto { get; set; }
        ZLJ.App.Common.OU.IOuAppService ouProviderAppService;

        protected ZLJ.App.Common.OU.IOuAppService OuProviderAppService => ouProviderAppService ?? ScopedServices.GetRequiredService<ZLJ.App.Common.OU.IOuAppService>();
        protected override void OnInitialized()
        {
            CreatedDto.Description = "xxxxxxxxxxxxxxx";
            base.OnInitialized();
        }

        List<TreeViewItem<ZLJ.App.Common.OU.OuDto>> items;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var list = await OuProviderAppService.GetListAsync(new App.Common.OU.GetListInput { Code = string.Empty, IsOnlyLoadChild = true });
            items= list.Select(c => new TreeViewItem<ZLJ.App.Common.OU.OuDto>(c) { Text=c.DisplayName, IsExpand=false, HasChildren=true }).ToList();
        }

        private async Task<IEnumerable<TreeViewItem<ZLJ.App.Common.OU.OuDto>>> ExpandNodeAsync(TreeViewItem<ZLJ.App.Common.OU.OuDto> node)
        {
            var current = node.Value;
            var list = await OuProviderAppService.GetListAsync(new App.Common.OU.GetListInput { Code = current.Code, IsOnlyLoadChild = true });
            return list.Select(c=>new TreeViewItem<ZLJ.App.Common.OU.OuDto>(c) { Text = c.DisplayName, IsExpand = false, HasChildren = true });
        }
    }
}