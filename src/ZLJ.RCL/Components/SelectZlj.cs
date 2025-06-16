using BXJG.Utils.Application.Share.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 抽象的选择框
    /// 默认实现搜索并加载前20行符合条件的数据
    /// </summary>
    /// <typeparam name="TItemValue"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class SelectZlj<TItemValue, TItem> : Select<TItemValue, TItem>
    {
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        protected virtual HttpClient HttpClient => HttpClientFactory.CreateHttpClientCommon();
        protected override void Dispose(bool disposing)
        {
            try
            {
                cts?.Cancel();
            }
            catch { }
            try
            {
                cts?.Dispose();
            }
            catch { }

            base.Dispose(disposing);
        }
        //  protected Task<PagedResultDto<TItem>> _oldTask;
        //protected bool _loading;

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
            parameters.SetParameterProperties(this);

            //  if (DataSource == null)
            //      DataSource = new List<TItem >();

            //无论是远程搜索，还是本地搜索，开启此属性后 才能输入

            if (parameters.TryGetValue<IEnumerable<TItem>>(nameof(DataSource), out var ds))
                throw new Exception("请设置DataSourceInit，而不是DataSource");

            if (!parameters.TryGetValue<bool>(nameof(EnableSearch), out var _))
                EnableSearch = true;

            if (!parameters.TryGetValue<bool>(nameof(AutoClearSearchValue), out var _))
                AutoClearSearchValue = false;

            if (!parameters.TryGetValue<bool>(nameof(EnableVirtualization), out var _))
                EnableVirtualization = true;

            //if (parameters.TryGetValue<IEnumerable<TItem>>(nameof(DataSourceInit), out var ds1))
            //{
            //    if (ds1 != DataSourceInit)
            //    {
            //        this.Logger.LogWarning("DataSourceInit变动了");
            //        DataSource = ds1;

            //        if (ds1.Count() >= MaxCount)
            //        {
            //            //if (!parameters.TryGetValue<bool>(nameof(EnableVirtualization), out var _))
            //            //    EnableVirtualization = true;
            //            if (EnableSearch && OnSearch == null)
            //                OnSearch = Search;
            //            await RemoteInit(parameters);
            //        }
            //        else
            //        {
            //            OnSearch = null;
            //            await LocalInit(parameters);
            //        }
            //    }
            //}
            //else //if(DataSource==null)
            //{
            //    //if (DataSource == null)
            //    //{
            //    //    var r = await HttpClient.GetAllProvider<TItem>(new PagedAndSortedResultRequest<object> { MaxResultCount = MaxCount, Filter = new { Keywords = string.Empty } });
            //    //    DataSource = r.Items;
            //    //}


            //    if (EnableSearch && OnSearch == null)
            //        OnSearch = Search;

            //    await RemoteInit(parameters);
            //}
            ////dic.TryAdd(nameof(DataSource), this.DataSource);

            await base.SetParametersAsync(ParameterView.Empty);
            // await base.SetParametersAsync(parameters);


        }



        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        protected ILogger Logger;

        /// <summary>
        /// 若数据量大于此值，则开启远程搜索，否则仅在本地搜索
        /// </summary>
        [Parameter]
        public int MaxCount { get; set; } = 50;
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
                OnSearch = Search;

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


        CancellationTokenSource cts = new CancellationTokenSource();


        //    public 
        [AbpExceptionInterceptor]
        void Search(string val)
        {
            Task.Run(async () =>
            {
                try
                {
                    await SearchCore(val);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "异步加载下拉框数据失败");
                }
            });
        }
        protected virtual async Task SearchCore(string? value = default)
        {
            //不要判断，肯定是希望输入后查询满足最新关键字的，而不是出现之前的查询
            //if (Loading)
            //    return;

            try
            {
                cts?.Cancel();
            }
            catch { }
            try
            {
                cts?.Dispose();
            }
            catch { }

            cts = new CancellationTokenSource();
            Loading = true;
            _ = InvokeAsync(StateHasChanged);
            try
            {
                var r = await HttpClient.GetAllProvider<TItem>(new { Filter = new { Keywords = value }, MaxResultCount = value.IsNullOrWhiteSpaceBXJG() ? MaxCount : int.MaxValue }, cancellationToken: cts.Token);

                //this.selected

                // var list = r.Items.ToList();
                //// base._s
                // try
                // {
                //     var r1 = await HttpClient.GetProvider<TItem>(new { Id = this._selectedValues.FirstOrDefault() }, cancellationToken: cts.Token);
                //     list.Add(r1);
                // }
                // catch { }


                DataSource = r.Items;
                await OnParametersSetAsync();//经过测试，必须调用它，列表才会显示新的数据
            }
            finally
            {
                Loading = false;
            }
            _ = InvokeAsync(StateHasChanged);//经过测试，这里必须调用
        }
    }

    public class SelectZljLong<TItem> : SelectZlj<long, TItem>
    {
    }
    public class SelectZljLongNull<TItem> : SelectZlj<long?, TItem>
    {
    }
    public class SelectZljGuid<TItem> : SelectZlj<Guid, TItem>
    {
    }
    public class SelectZljGuidNull<TItem> : SelectZlj<Guid?, TItem>
    {
    }
    public class SelectZljString<TItem> : SelectZlj<string, TItem>
    {
    }

}
