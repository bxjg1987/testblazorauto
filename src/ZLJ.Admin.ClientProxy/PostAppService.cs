using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Share.Auditing.Dto;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Post;
using ZLJ.Application.Common.ClientProxy;
using BXJG.Utils.Application.Share;
using BXJG.Common.Contracts;

namespace ZLJ.Admin.ClientProxy
{
    public class PostAppService: BaseAppServiceClient,IPostAppService
    {
        public PostAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<BatchOperationOutput<int>> BatchDeleteAsync(BatchOperationInput<int> input)
        {
            return await Post<BatchOperationOutput<int>>("api/services/app/post/BatchDelete", input);
        }

        public async Task<PostDto> CreateAsync(CreatePostDto input)
        {
            return await Post<PostDto>("api/services/app/post/Create", input);
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
             await Post("api/services/app/post/Delete", input);
        }

        public async Task<PagedResultDto<PostDto>> GetAllAsync(PagedAndSortedResultRequest<PagedPostResultRequestDto> input)
        {
            return await Post<PagedResultDto<PostDto>>("api/services/app/post/getall", input);
        }

        public async Task<PostDto> GetAsync(EntityDto<int> input)
        {
            return await Post<PostDto>("api/services/app/post/Get", input);
        }

        //public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        //{
        //    if (input.MaxResultCount <= 0)
        //        input.MaxResultCount = 20;
        //    //最好把这个方法变成post的，传参更简单，或把api整体配置为post
        //    return await Post<PagedResultDto<AuditLogListDto>>("api/services/app/AuditLog/GetAuditLogs", input);
        //}

        public async Task<PostDto> UpdateAsync(PostEditDto input)
        {
            return await Post<PostDto>("api/services/app/post/Update", input);
        }
    }
}
