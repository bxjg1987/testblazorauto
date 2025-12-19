using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Kehu;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 抽象的选择框
    /// 默认实现搜索并加载前20行符合条件的数据
    /// </summary>
    /// <typeparam name="TItemValue"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class SelectZlj<TItemValue, TItem> : Select<TItemValue, TItem> //where TItem : EntityDto< IEntityDto<ob>>
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
        /// 初始数据，若数量小于MaxCount，则仅本地搜索
        /// 一般列表渲染时，仅保留一行处于编辑状态，能提高渲染性能，所以通常没必要设置这个初始值
        /// 除非一个组件中初始时要渲染大量下拉框
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSourceInit { get; set; }


        public override async Task SetParametersAsync(ParameterView parameters)
        {
            //  var dic = parameters.ToDictionary();

            // 检查是否传入了DataSource参数，如果传入了就抛出异常
            if (parameters.TryGetValue<IEnumerable<TItem>>(nameof(DataSource), out var ds))
                throw new Exception("请设置DataSourceInit，而不是DataSource");

            // 设置默认值（仅当参数未提供时）
            var hasEnableSearch = parameters.TryGetValue<bool>(nameof(EnableSearch), out var enableSearch);
            var hasAutoClearSearchValue = parameters.TryGetValue<bool>(nameof(AutoClearSearchValue), out var autoClearSearchValue);
            var hasEnableVirtualization = parameters.TryGetValue<bool>(nameof(EnableVirtualization), out var enableVirtualization);

            // 调用父类方法，让父类处理参数设置
            await base.SetParametersAsync(parameters);

            // 仅当参数未提供时设置默认值
            if (!hasEnableSearch)
                EnableSearch = true;
            if (!hasAutoClearSearchValue)
                AutoClearSearchValue = false;
            if (!hasEnableVirtualization)
                EnableVirtualization = true;


        }

        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //    this.Search(default);
        //}

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        protected ILogger Logger;

        /// <summary>
        /// 若数据量大于此值，则开启远程搜索，否则仅在本地搜索
        /// </summary>
        [Parameter]
        public int MaxCount { get; set; } = 5;
        [AbpExceptionInterceptor]
        protected override void OnInitialized()
        {
            Logger = LoggerFactory.CreateLogger(GetType());
            if (ValueName.IsNullOrWhiteSpaceBXJG())
                ValueName = "Id";

            if (LabelName.IsNullOrWhiteSpaceBXJG())
                LabelName = "Name";
            DataSource = DataSourceInit;

            //上面的id和名称必须先设置，否则报错

            base.OnInitialized();
        }

        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            if (DataSource == null || DataSource.Any() == false)
            {
                await SearchCore();
            }

            if (EnableSearch && OnSearch == null && DataSource.Count() >= MaxCount)
                OnSearch = Search;// Search;

            await base.OnInitializedAsync();
        }
        //protected virtual ValueTask LocalInit(ParameterView parameters)
        //{
        //    return ValueTask.CompletedTask;
        //}
        //protected virtual ValueTask RemoteInit(ParameterView parameters)
        //{
        //    return ValueTask.CompletedTask;
        //}




        //    public 
        // [AbpExceptionInterceptor]
        public void Search(string val)
        {
            //  Task.Run(async () =>
            // {
            //    try
            //  {
            _ = SearchCore(val);
            //  }
            //   catch (Exception ex)
            //  {
            //       Logger.LogError(ex, "异步加载下拉框数据失败");
            //   }
            // });
        }
        protected virtual async Task SearchCore(string? value = default)
        {
            //不要判断，肯定是希望输入后查询满足最新关键字的，而不是出现之前的查询
            //if (Loading)
            //    return;

            //Loading = true;
            //await InvokeAsync(StateHasChanged);
            //try
            //{
            dynamic tj = GetCondition();
            try
            {
                tj.Keywords = value;
            }
            catch { }
            var r = await HttpClient.GetAllProvider<TItem>(new { Filter = tj, MaxResultCount = value.IsNullOrWhiteSpaceBXJG() ? MaxCount : int.MaxValue });

            DataSource = r.Items;
            if (Value != null && !Value.Equals(default) && !r.Items.Any(d => d.GetFieldOrPropertyValue("Id").Equals(Value)))
            {
                if (!Value.Equals(curr?.GetFieldOrPropertyValue("Id")))
                {
                    var r1 = await HttpClient.GetProvider<TItem>(new { Id = Value });
                    curr = r1;
                }
                // 直接在原列表上添加，避免不必要的复制
                if (curr != null)
                {
                    DataSource = r.Items.Concat([curr]);
                }
            }
            await OnParametersSetAsync();//经过测试，必须调用它，列表才会显示新的数据
                                         //}
                                         //finally
                                         //{
                                         //    Loading = false;
                                         //}
                                         // 统一在所有状态更新后调用，确保组件正确渲染
                                         // await OnParametersSetAsync();//经过测试，必须调用它，列表才会显示新的数据
                                         // 调用父类的SetClassMap方法重新计算CSS类，确保Loading状态正确更新
                                         //await InvokeAsync(() => {
                                         //    // 反射调用父类的SetClassMap方法
                                         //    var method = typeof(Select<TItemValue, TItem>).GetMethod("SetClassMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                         //    method?.Invoke(this, null);
                                         //    StateHasChanged();
                                         //});
        }
        TItem curr;

        protected virtual dynamic GetCondition()
        {
            return new ExpandoObject();
        }
    }
    public class SelectZlj<TItemValue, TItem, TCondtion> : SelectZlj<TItemValue, TItem> where TCondtion : new()
    {

        [Parameter]
        public virtual TCondtion Condtion { get; set; } = new TCondtion();
        protected override dynamic GetCondition()
        {
            return Condtion;
        }
    }

    //下面的有点过度封装了

    public class SelectZljLong<TItem> : SelectZlj<long, TItem> //where TItem : EntityDto<long>
    {
    }
    public class SelectZljLongNull<TItem> : SelectZlj<long?, TItem> //where TItem : EntityDto<long?>
    {
    }
    public class SelectZljGuid<TItem> : SelectZlj<Guid, TItem> //where TItem : EntityDto<Guid>
    {
    }
    public class SelectZljGuidNull<TItem> : SelectZlj<Guid?, TItem> //where TItem : EntityDto<Guid?>
    {
    }
    public class SelectZljString<TItem> : SelectZlj<string, TItem> //where TItem : EntityDto<string>
    {
    }

}
