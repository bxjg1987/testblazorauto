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
        int total = 1;

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
            if (condition.Sorting.IsNullOrWhiteSpaceBXJG())
                condition.Sorting = "Id";

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
    }
}
