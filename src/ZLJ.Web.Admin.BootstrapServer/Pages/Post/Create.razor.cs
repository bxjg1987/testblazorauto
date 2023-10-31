using BootstrapBlazor.Components;
using BXJG.AbpBootstrapBlazor.Interceptors;
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
        ZLJ.App.Common.OU.IOuAppService ouProviderAppService;

        protected ZLJ.App.Common.OU.IOuAppService OuProviderAppService => ouProviderAppService ?? ScopedServices.GetRequiredService<ZLJ.App.Common.OU.IOuAppService>();
        protected override void OnInitialized()
        {
       
            base.OnInitialized();
        }

        List<TreeViewItem<long>> items=new List<TreeViewItem<long>>();

        [AbpBBException]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var list = await OuProviderAppService.GetListAsync(new App.Common.OU.GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
            items = Fill(list);
        }

        private List<TreeViewItem<long>> Fill(IList<App.Common.OU.OuDto> children, TreeViewItem<long> parent = default)
        {
            List<TreeViewItem<long>> list = new List<TreeViewItem<long>>();
            foreach (var item in children)
            {
                var dto = new TreeViewItem<long>(item.Id)
                {
                    IsExpand = false,
                    Text = item.DisplayName,
                    Parent = parent,
                    HasChildren = item.ChildrenCount > 0,
                    Value = item.Id,
                };
                if (item.ChildrenCount > 0)
                {
                    dto.Items = Fill(item.Children, dto);
                }
                list.Add(dto);
            }
            return list;
        }

        //private async Task<IEnumerable<TreeViewItem<ZLJ.App.Common.OU.OuDto>>> ExpandNodeAsync(TreeViewItem<ZLJ.App.Common.OU.OuDto> node)
        //{
        //    var current = node.Value;
        //    var list = await OuProviderAppService.GetListAsync(new App.Common.OU.GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        //    return list.Select(c=>new TreeViewItem<ZLJ.App.Common.OU.OuDto>(c) { Text = c.DisplayName, IsExpand = false, HasChildren = true });
        //}
    }
}