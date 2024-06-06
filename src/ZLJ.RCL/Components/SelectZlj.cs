using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Kehu;

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

        //  protected Task<PagedResultDto<TItem>> _oldTask;
        //protected bool _loading;



        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (ValueName.IsNullOrWhiteSpaceBXJG())
                ValueName = "Id";

            if (LabelName.IsNullOrWhiteSpaceBXJG())
                LabelName = "Name";

            //  if (DataSource == null)
            //      DataSource = new List<TItem >();

            //无论是远程搜索，还是本地搜索，开启此熟悉后 才能输入
            EnableSearch = true;




            return base.SetParametersAsync(ParameterView.Empty);
            // await base.SetParametersAsync(parameters);


        }

        /// <summary>
        /// 若数据量大于此值，则开启远程搜索，否则仅在本地搜索
        /// </summary>
        [Parameter]
        public int MaxCount { get; set; } = 100;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (DataSource == null || !DataSource.Any())
            {
                var r = await HttpClient.GetAllProvider<TItem>(new PagedAndSortedResultRequest<object> { MaxResultCount = MaxCount, Filter = new { Keywords = string.Empty } });
                DataSource = r.Items;
            }

            if (DataSource.Count() >= MaxCount)
                if (EnableSearch && OnSearch == null)
                    OnSearch = async a => Search(a);
        }
        CancellationTokenSource cts = new CancellationTokenSource();
        async Task Search(string value)
        {
            //不要判断，肯定是希望输入后查询满足最新关键字的，而不是出现之前的查询
            //if (Loading)
            //    return;

            cts?.Cancel();
            cts = new CancellationTokenSource();
            Loading = true;
            try
            {
                var r = await HttpClient.GetAllProvider<TItem>(new { Filter = new { Keywords = value } }, cancellationToken: cts.Token);
                DataSource = r.Items;
                OnParametersSet();//经过测试，必须调用它，列表才会显示新的数据
            }
            finally
            {
                Loading = false;
            }
            StateHasChanged();//经过测试，这里必须调用
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
