using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.UI;
using AntDesign.TableModels;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Auditing.Dto;
using ZLJ.RCL.Interceptors;

namespace ZLJ.Admin.CoreRCL.Systemlog
{
    public partial class List
    {
        IAuditLogAppService appService;

        Table<AuditLogListDto> table;
        GetAuditLogsInput condition = new GetAuditLogsInput();
        AuditLogListDto[] data = Array.Empty<AuditLogListDto>();
        int total = 0;
        bool isLoading;
        //protected override Task OnInitializedAsync()
        //{
        //    return base.OnInitializedAsync();
        //}
        // [AbpExceptionInterceptor]

        protected override void OnInitialized()
        {
            appService = ScopedServices.GetRequiredService<IAuditLogAppService>();
            // throw new UserFriendlyException("xxxxxxxxxxxxx");
        }

#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        async Task OnChange(QueryModel<AuditLogListDto> queryModel)
        {
            //await Task.Delay(2000);
            //data = [
            //   new AuditLogListDto { Id = 3, BrowserInfo = "xxxxxx" }
            //   ];
            //total = 1;

            //有bug，静态渲染时，无论是否开启流式渲染 界面都无法显示数据，server模式是ok的



            condition.SkipCount = (queryModel.PageIndex - 1) * queryModel.PageSize;
            condition.MaxResultCount = queryModel.PageSize;
            // await Console.Out.WriteLineAsync(   System.Text.Json.JsonSerializer.Serialize(queryModel.SortModel));
            condition.Sorting = string.Empty;
            foreach (var item in queryModel.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority))
            {
                condition.Sorting += $"{item.FieldName} {item.Sort.TrimEnd("end".ToArray())},";
            }
            if (condition.Sorting.IsNullOrWhiteSpaceBXJG())
                condition.Sorting = "ExecutionTime desc";
            else
                condition.Sorting = condition.Sorting.TrimEnd(',');
            // if (condition.Sorting.IsNullOrWhiteSpaceBXJG())
            //       condition.Sorting = "ExecutionTime desc";

            var r = await appService.GetAuditLogs(condition);

            // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(r));
            //  table.DataSource = r.Items;

            data = r.Items.ToArray();
            total = r.TotalCount;
            // if(table!=default)
            //     table.ReloadData();
            // queryModel.CurrentPagedRecords(data.AsQueryable());  
            //StateHasChanged();
        }
        //DatePicker<DateTime?> kssj;
        // int pageIndex = 1;
    
        private void Search()
        {
            //pageIndex = 1;
            table.PageIndex = 1;
            var qm = table.GetQueryModel();
            //qm.FilterModel.Clear();//这里清不清没啥意义，因为我们使用的查询模型
            table.ReloadData(qm);
            StateHasChanged();
           //kssj.Focus();
        }

        void BtnReLoadClick() {
            condition = new GetAuditLogsInput();
            Search();
        }
    }
}