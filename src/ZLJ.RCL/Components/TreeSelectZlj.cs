using AntDesign;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /*
     * 由于抽象应用服务限制，这里必须提供这些泛型
     * 使用继承，这样所有调用方可以继续使用TreeSelect原生的属性，若包一层会失去这样的能力。
     * 由于我们的树是抽象的，对应提供此抽象下拉树组件，各具体树形数据最好提供对应的子类组件。
     * 有时候可能仅需要选择某个节点下的子节点集合，而不是树形的，为了统一，统一使用树控件。
     * 提供异步加载数据的委托属性，且默认从应用服务加载，调用方可以自己提供，调用方页可以传递原生的DataSource属性
     * 
     * selectTree查看文档时要同时结合select的文档
     */

    /// <summary>
    /// 参考开发随记中的设计思路
    /// 这是泛型的、通用的、统一树形的 下拉树选择框，它不限定是用在搜索（重在事件触发数据加载）还是表单（重在双向绑定）
    /// 推荐所有树形数据的选择都继承它
    /// </summary>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    public class TreeSelectZlj<TGetTreeForSelectOutput> : TreeSelect<TGetTreeForSelectOutput> where TGetTreeForSelectOutput : IGeneralTree<TGetTreeForSelectOutput>
        //where TGetNodesForSelectOutput : ComboboxItemDto 由于我们规定了统一使用树，所以这个约束没有必要
    {
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        protected virtual HttpClient HttpClient => HttpClientFactory.CreateHttpClientCommon();

        /// <summary>
        /// 只加载此节点下的后代节点
        /// 不用用code，因为code会变
        /// </summary>
        [Parameter]
        public virtual long? ParentId { get; set; }
        /// <summary>
        /// true绑定id，false绑定code
        /// </summary>
        [Parameter]
        public virtual bool IsKeyId { get; set; }


        [Parameter]
        public long TreeId { get; set; }

        [Parameter]
        public EventCallback<long> TreeIdChanged { get; set; }


        [Parameter]
        public long? TreeIdNullable { get; set; }

        [Parameter]
        public EventCallback<long?> TreeIdNullableChanged { get; set; }

        protected override void OnCurrentValueChange(string value)
        {
            base.OnCurrentValueChange(value);
            //this.InvokeValuesChanged
            //_ InvokeAsync(async()=>{
            //   await TreeIdNullableChanged.InvokeAsync(1);
            //});

            long? z = default;
            if (value.IsNullOrWhiteSpaceBXJG() || value == "0")
                z = default;
            else
                z = long.Parse(value);

            InvokeAsync(async () =>
            {
                await TreeIdNullableChanged.InvokeAsync(z);
                await TreeIdChanged.InvokeAsync(z.HasValue ? z.Value : default);
                StateHasChanged();
            });
        }

        //protected virtual ValueTask SetDefault(IReadOnlyDictionary<string, object> dic) => ValueTask.CompletedTask;
        //protected override void OnValueChange(string value)
        //{
        //    //  base.OnValueChange(value);
        //    ValueChanged.InvokeAsync(value);
        //}
        //protected override void OnCurrentValueChange(string value)
        //{
        //    Value = value;


        //    CurrentValue = value;

        //    //base.OnCurrentValueChange(value);
        //}
        //// override value
        //void ClearSelected()
        //{
        //    //this.Value = string.Empty;
        //    //OnCurrentValueChange(string.Empty);
        //    Value = string.Empty;
        //    CurrentValue = string.Empty;

        //    //this.SetValueAsync
        //    ValueChanged.InvokeAsync().ContinueWith(t =>
        //    {
        //        InvokeOnSelectedItemChanged();
        //        Console.WriteLine("xxxxxxxxxxxxx");
        //    });
        //    // InvokeOnSelectedItemChanged();
        //    // OnCurrentValueChange(string.Empty);
        //    //StateHasChanged();
        //    // _ = ValueChanged.InvokeAsync(string.Empty);
        //}
        //[Inject]
        //public IServiceProvider ServiceProvider { get; set; }

#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (TitleExpression == default)
                TitleExpression = node => node.DataItem.DisplayName;

            if (ChildrenExpression == default)
                ChildrenExpression = node => node.DataItem.Children;

            if (KeyExpression == default)
            {
                if (IsKeyId)
                    KeyExpression = node => node.DataItem.Id == 0 ? default : node.DataItem.Id.ToString();
                else
                    KeyExpression = node => node.DataItem.Code;
            }

            if (IsLeafExpression == default)
                IsLeafExpression = node => node.DataItem.Children == null || !node.DataItem.Children.Any();

            if (DataSource == null)
            {
                //await using var service = ServiceProvider.CreateAsyncScope();
                //var appService = service.ServiceProvider.GetRequiredService<TAppService>();
                // DataSource = await appService.GetTreeForSelectAsync(new TGetTreeForSelectInput { ParentId = ParentId });
                DataSource = await HttpClient.GetTreeForSelect<TGetTreeForSelectOutput>(new { ParentId });
            }
        }

        //protected abstract HttpClient CreateHttpClient();
        //[AbpExceptionInterceptor]
        //async Task Search(string str)
        //{
        //    //str = str ?? ParentId?.ToString();
        //    //由于当前组件继承了TreeSelect，无法继承blazor自带的OwnComponentBase，所以这里自己通过ServiceProvider拿服务，注意创建范围
        //    //范围使用异步版本，参考：https://andrewlock.net/exploring-dotnet-6-part-10-new-dependency-injection-features-in-dotnet-6/
        //    //In general, if you're manually creating scopes in your application (as in the above example), it seems to me that you should use CreateAsyncScope() wherever possible.
        //    await using var service = ServiceProvider.CreateAsyncScope();
        //    var appService = service.ServiceProvider.GetRequiredService<TAppService>();
        //    DataSource = await appService.GetTreeForSelectAsync(new TGetTreeForSelectInput {  });
        //}
    }
}