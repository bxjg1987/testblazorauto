using Abp.Extensions;
using Abp.Reflection.Extensions;
using AntDesign;
using AutoMapper.Internal;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /*
     * 使用继承，这样所有调用方可以继续使用TreeSelect原生的属性，若包一层会失去这样的能力。
     * 由于我们的树是抽象的，对应提供此抽象下拉树组件，各具体树形数据最好提供对应的子类组件。
     * 有时候可能仅需要选择某个节点下的子节点集合，而不是树形的，为了统一，统一使用树控件。
     * 提供异步加载数据的委托属性，且默认从应用服务加载，调用方可以自己提供，调用方页可以传递原生的DataSource属性
     * 
     * selectTree查看文档时要同时结合select的文档
     */

    //有时候需要绑定long? 有时候需要绑定long 有时候需要绑定code，索性用string作为value，然后提供两个外挂属性绑定

    /// <summary>
    /// 参考开发随记中的设计思路
    /// 这是泛型的、通用的、统一树形的 下拉树选择框，它不限定是用在搜索（重在事件触发数据加载）还是表单（重在双向绑定）
    /// 推荐所有树形数据的选择都继承它
    /// </summary>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    public class TreeSelectZlj<TGetTreeForSelectOutput> : TreeSelect<string, TGetTreeForSelectOutput> where TGetTreeForSelectOutput : IGeneralTree<TGetTreeForSelectOutput>
        //where TGetNodesForSelectOutput : ComboboxItemDto 由于我们规定了统一使用树，所以这个约束没有必要
    {
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        protected virtual HttpClient HttpClient => HttpClientFactory.CreateHttpClientCommon();
        /// <summary>
        /// 专门给肉夹馍aop用的，你不该调用这个
        /// </summary>
        [Inject]
        public IServiceProvider Services { get; set; }
        /// <summary>
        /// 只加载此节点下的后代节点
        /// 不用用code，因为code会变
        /// </summary>
        [Parameter]
        public virtual string? ParentName { get; set; }
        string parentName;
        /// <summary>
        /// true绑定id，false绑定code
        /// </summary>
        [Parameter]
        public virtual bool IsKeyId { get; set; } = true;

        // 用于防止循环调用的标志位
        //private bool _updatingValueFromPropertySetter = false;

        long? treeId;

        [Parameter]
        public long TreeId
        {
            get
            {
                //if (Value.IsNullOrWhiteSpaceBXJG())
                //    return default;
                //if (long.TryParse(Value, out long result))
                //    return result;
                //return default;
                return (long)treeId;
            }
            set
            {
                // 防止循环调用
                //if (_updatingValueFromPropertySetter)
                //    return;

                if (value == treeId)
                    return;


                //if (Value.IsNullOrEmpty() && value == 0)
                //    return;

                // string newValue = value == 0 ? string.Empty : value.ToString();

                //_updatingValueFromPropertySetter = true;
                //try
                //{
                treeId = value;
                CurrentValue = value == 0 ? string.Empty : value.ToString();//newValue
                //}
                //finally
                //{
                //    _updatingValueFromPropertySetter = false;
                //}
            }
        }

        [Parameter]
        public EventCallback<long> TreeIdChanged { get; set; }


        [Parameter]
        public long? TreeIdNullable
        {
            get
            {
                return treeId;
                //if (Value.IsNullOrWhiteSpaceBXJG() || Value == "0")
                //    return default;
                //if (long.TryParse(Value, out long result))
                //    return result;
                //return default;
            }
            set
            {
                // 防止循环调用
                //if (_updatingValueFromPropertySetter)
                //    return;

                if (value == treeId)
                    return;
                //if (Value.IsNullOrEmpty() && (!value.HasValue || value == 0))
                //    return;

                //string newValue = (value == 0 || value == null) ? string.Empty : value.ToString();

                //_updatingValueFromPropertySetter = true;
                //try
                //{
                treeId = value;
                CurrentValue = (value == 0 || value == null) ? string.Empty : value.ToString(); // newValue;
                //}
                //finally
                //{
                //    _updatingValueFromPropertySetter = false;
                //}
            }
        }

        [Parameter]
        public EventCallback<long?> TreeIdNullableChanged { get; set; }

        //  override onvalu
        protected override void OnCurrentValueChange(string value)
        {
            //if (value.IsNullOrWhiteSpace() && Value.IsNullOrWhiteSpace())
            //    return;


            //var sdfsdf = this.Value;
            //var sdsss = this.CurrentValue;
            // var sss = this.CurrentValueAsString;


            //if (Value == value)
            //    return;

            // 防止在属性setter中触发的OnCurrentValueChange再次触发事件回调
            //if (_updatingValueFromPropertySetter)
            //    return;

            base.OnCurrentValueChange(value);

            //long? z = default;
            //if (!value.IsNullOrWhiteSpaceBXJG() && value != "0" && long.TryParse(value, out long parsedValue))
            //    z = parsedValue;

            _ = InvokeAsync(async () =>
            {
                if (TreeIdNullableChanged.HasDelegate)
                    await TreeIdNullableChanged.InvokeAsync(value.IsNullOrEmpty() || value == "0" ? null : long.Parse(value));
                if (TreeIdChanged.HasDelegate)
                    await TreeIdChanged.InvokeAsync(value.IsNullOrEmpty() || value == "0" ? default : long.Parse(value));
            });
        }

        IEnumerable<long> treeIds = Enumerable.Empty<long>();
        [Parameter]
        public IEnumerable<long> TreeIds
        {
            get
            {
                return treeIds;
                //if (Values == null || !Value.Any())
                //    return default;

                //return Values.Select(x => long.Parse(x));
            }
            set
            {
                //OnValuesChangeAsync
                // 防止循环调用
                //if (_updatingValueFromPropertySetter)
                //    return;

                if (value == null && treeIds == null)
                    return;

                if (value.SequenceEqual(treeIds))
                    return;

                //var newValue = value == null || !value.Any() ? Enumerable.Empty<string>() : value.Select(x => x.ToString());
                //if (Values == null && value != null || newValue.Count() != Values.Count() || newValue.Any(x => !Values.Contains(x)))
                //{
                //_updatingValueFromPropertySetter = true;
                //try
                //{
                treeIds = value;
                Values = value == null || !value.Any() ? Enumerable.Empty<string>() : value.Select(x => x.ToString()); //newValue;
                //}
                //finally
                //{
                //    _updatingValueFromPropertySetter = false;
                //}
                //}
            }
        }
        //OnParametersSetAsync中触发
        [Parameter]
        public EventCallback<IEnumerable<long>> TreeIdsChanged { get; set; }

        //public override IEnumerable<string> Values { get => base.Values; set { 
        //        base.Values = value;
        //    }

        //}



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



        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

            if (TitleExpression == default)
                TitleExpression = node => node.DataItem.DisplayName;

            if (ChildrenExpression == default)
                ChildrenExpression = node => node.DataItem.Children;

            if (KeyExpression == default)
            {
                //if (IsKeyId)
                //    KeyExpression = node => node.DataItem.Id == 0 ? default : node.DataItem.Id.ToString();
                //else
                //    KeyExpression = node => node.DataItem.Code;
                KeyExpression = node => node.DataItem.Id.ToString();
            }



            if (ItemValue == default)
            {
                if (IsKeyId)
                    ItemValue = x => x.Id.ToString();
                else
                    ItemValue = x => x.Code;
            }

            if (IsLeafExpression == default)
                IsLeafExpression = node => node.DataItem.Children == null || !node.DataItem.Children.Any();

            if (DataSource == null)
            {
                await LoadDataSource();
                //await using var service = ServiceProvider.CreateAsyncScope();
                //var appService = service.ServiceProvider.GetRequiredService<TAppService>();
                // DataSource = await appService.GetTreeForSelectAsync(new TGetTreeForSelectInput { ParentId = ParentId });
                //DataSource = await HttpClient.GetTreeForSelect<TGetTreeForSelectOutput>(new { ParentName });
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            // 订阅基类的 ValuesChanged 事件
            if (!ValuesChanged.HasDelegate && TreeIdsChanged.HasDelegate)
            {
                //var originalCallback = ValuesChanged;
                ValuesChanged = EventCallback.Factory.Create<IEnumerable<string>>(this, async values =>
                {
                    // 先执行原始回调
                    //await originalCallback.InvokeAsync(values);

                    // 再执行自定义逻辑
                    await TreeIdsChanged.InvokeAsync(values == null ? null : values.Select(x => long.Parse(x)));

                    // 这里可以添加你的自定义逻辑
                    //Console.WriteLine($"Values changed: {string.Join(", ", values ?? Enumerable.Empty<TItemValue>())}");
                });
            }
            //init加载就行了
            //if (ParentName != parentName)
            //{
            //    await LoadDataSource();
            //}
        }

        //public override async Task SetParametersAsync(ParameterView parameters)
        //{

        //    await LoadDataSource();
        //    //base.ForceUpdateValueString
        //}
        /// <summary>
        /// dfgdg
        /// </summary>
        [Parameter]
        public EventCallback OnAsyncLoaded { get; set; }
        protected virtual async Task LoadDataSource()
        {
            DataSource = await HttpClient.GetTreeForSelect<TGetTreeForSelectOutput>(new { ParentName });
            parentName = ParentName;

            //DefaultValues = treeIds.Select(x=>x.ToString()); //DataSource.Select(d=>d.)

            //base.Value = treeId?.ToString();
            ////StateHasChanged();
            // base.Values = treeIds?.Select(x => x.ToString());
            //StateHasChanged();
            //return;
            //this.Values=
            //_ = InvokeAsync(async () =>
            //{
            //if (Value.IsNotNullOrWhiteSpaceBXJG() || Values != null)
            //{
            //if (RendererInfo.IsInteractive)

            //反射获取当前对象字段_cachedValues的值

            //await OpenAsync();
            //StateHasChanged();
            //if (RendererInfo.IsInteractive)
            //{
            //异步加载数据以后，开合下下拉框，否则后续的反射执行将无效
            const string sdfsf = "display:none;";
            DropdownStyle += sdfsf;
            await OpenAsync();
            //Open = true;
            //StateHasChanged();
            //await Task.Delay(5000);
            // Open = false;
            //StateHasChanged();
            DropdownStyle = DropdownStyle.Replace(sdfsf, string.Empty);
            await _dropDown.Close();
            // }



            //   await Task.Delay(1);

            //var valuesField = GetType().GetPrivateField("_cachedValues");
            //var valueField = GetType().GetPrivateField("_cachedValue");

            //valuesField.SetValue(this, Enumerable.Empty<string>());
            //valueField.SetValue(this, string.Empty);
            //StateHasChanged();

            //// var values = this.GetFieldValue("_cachedValues");// GetType().GetField("_cachedValues", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            //// Abp.Reflection.Extensions.MemberInfoExtensions.fi
            //// var value = this.GetFieldValue("_cachedValue");//.GetField("_cachedValue", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

            //base.Value = treeId?.ToString();
            ////StateHasChanged();
            //base.Values = treeIds?.Select(x => x.ToString());
            //StateHasChanged();
            //return;



            //   await Task.Delay(1000);
            //反射调用父类的UpdateValueAndSelection
            //OnFirstAfterRenderAsync
            if (Multiple)
            {
                //var _tree = this.GetFieldValue("_tree"); //GetType().GetPrivateField("_tree").GetValue(this);
                //var treeNodes = _tree.GetFieldValue("_allNodes");
                //var _cachedValues = this.GetFieldValue("_cachedValues") as long[];
                //UpdateValuesSelection
                //反射时必须用父类类型，否则获取不到
                var methodInfo = typeof(TreeSelect<string, TGetTreeForSelectOutput>).GetMethod("UpdateValuesSelection",
                                   BindingFlags.NonPublic | BindingFlags.Instance);
                //Console.WriteLine($"反射执行UpdateValuesSelection：{methodInfo == null} treeNodes：{treeNodes==null} _cachedValues：{_cachedValues?.Any()}");
                methodInfo.Invoke(this, null);
                if (OnAsyncLoaded.HasDelegate)
                {
                    await OnAsyncLoaded.InvokeAsync();
                }
                StateHasChanged();
            }
            else if (OnAsyncLoaded.HasDelegate)
            {
                await OnAsyncLoaded.InvokeAsync();
                StateHasChanged();
            }

            //else
            //{
            //经过测试，单选时，开合下拉框即可
            //if (RendererInfo.IsInteractive)
            //{
            //const string sdfsf = "display:none;";
            //DropdownStyle += sdfsf;
            //await OpenAsync();
            ////Open = true;
            ////StateHasChanged();
            ////await Task.Delay(5000);
            //// Open = false;
            ////StateHasChanged();
            //DropdownStyle = DropdownStyle.Replace(sdfsf, string.Empty);
            //await _dropDown.Close();
            //}
            //else
            //{
            //    var methodInfo = typeof(TreeSelect<string, TGetTreeForSelectOutput>).GetMethod("UpdateValueAndSelection",
            //                  BindingFlags.NonPublic | BindingFlags.Instance);
            //   //Console.WriteLine("反射执行UpdateValueAndSelection：" + methodInfo == null);
            //    methodInfo.Invoke(this, null);
            //    StateHasChanged();
            //}
            //}

            //    StateHasChanged();
            // await Task.Delay(2000);
            //StateHasChanged();


            //}
            //});
            //StateHasChanged();
            //await FocusAsync();
            //await InvokeAsync(async () =>
            //{
            //    //反射调用父类的UpdateValueAndSelection
            //    var methodInfo = typeof(TreeSelect<string, TGetTreeForSelectOutput>)
            //           .GetMethod("UpdateValueAndSelection",
            //                      BindingFlags.NonPublic | BindingFlags.Instance);
            //    methodInfo.Invoke(this, null);
            //    StateHasChanged();
            //    await Task.Delay(2000);
            //    StateHasChanged();
            //});


            //  var old = Value;
            //await   base.OnFirstAfterRenderAsync();
            //  StateHasChanged();
            //  await base.OnFirstAfterRenderAsync();
            //  CurrentValue = old;
            //  await base.OnFirstAfterRenderAsync();
            //  StateHasChanged();
            //await base.OnFirstAfterRenderAsync();

            //ForceUpdateValueString(Value);
            //CurrentValue = Value;
            //_ = InvokeAsync(async () =>
            // {
            //  await Task.Delay(2000);
            //var sf = Value;
            //Value = "4";

            //    var sfsfd = base.Nodes;
            //  base.ExpandAll();
            //      await OnFirstAfterRenderAsync();
            //   var sdfsd = Value;
            //  CurrentValue=string.Empty;

            //  await Task.Delay(1000);
            //  CurrentValue = sdfsd;

            // });
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