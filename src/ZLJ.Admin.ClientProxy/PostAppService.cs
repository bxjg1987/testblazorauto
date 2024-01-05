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
using BXJG.Common.Dto;
using BXJG.Utils.Application.Share;

namespace ZLJ.Admin.ClientProxy
{
    public class PostAppService: BaseAppServiceClient,IPostAppService
    {
        public PostAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Task<BatchOperationOutput<int>> BatchDeleteAsync(BatchOperationInput<int> input)
        {
            throw new NotImplementedException();
        }

        public async Task<PostDto> CreateAsync(CreatePostDto input)
        {
            return await Post<PostDto>("api/services/app/post/Create", input);
        }

        public Task DeleteAsync(EntityDto<int> input)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<PostDto>> GetAllAsync(PagedAndSortedResultRequest<PagedPostResultRequestDto> input)
        {
            return await Post<PagedResultDto<PostDto>>("api/services/app/post/getall", input);
        }

        public Task<PostDto> GetAsync(EntityDto<int> input)
        {
            throw new NotImplementedException();
        }

        //public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        //{
        //    if (input.MaxResultCount <= 0)
        //        input.MaxResultCount = 20;
        //    //最好把这个方法变成post的，传参更简单，或把api整体配置为post
        //    return await Post<PagedResultDto<AuditLogListDto>>("api/services/app/AuditLog/GetAuditLogs", input);
        //}

        public Task<PostDto> UpdateAsync(PostEditDto input)
        {
            throw new NotImplementedException();
        }
    }
}
