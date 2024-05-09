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
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Dtos;

namespace ZLJ.Admin.ClientProxy
{
    public class PostAppService: IPostAppService
    {
        HttpClient httpClient;
        public PostAppService(IHttpClientFactory httpClientFactory) 
        {
            httpClient = httpClientFactory.CreateHttpClientAdmin();
        }

        public async Task<BatchOperationOutput<int>> DeleteBatchAsync(BatchOperationInput<int> input)
        {
            return await httpClient.Post<BatchOperationOutput<int>>("post/BatchDelete", input);
        }

        public async Task<PostDto> CreateAsync(CreatePostDto input)
        {
            return await httpClient.Post<PostDto>("post/Create", input);
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            await httpClient.Post("post/Delete", input);
        }

        public async Task<PagedResultDto<PostDto>> GetAllAsync(PagedAndSortedResultRequest<PagedPostResultRequestDto> input)
        {
            return await httpClient.Post<PagedResultDto<PostDto>>("post/getall", input);
        }

        public async Task<PostDto> GetAsync(EntityDto<int> input)
        {
            return await httpClient.Post<PostDto>("post/Get", input);
        }

        //public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        //{
        //    if (input.MaxResultCount <= 0)
        //        input.MaxResultCount = 20;
        //    //最好把这个方法变成post的，传参更简单，或把api整体配置为post
        //    return await Post<PagedResultDto<AuditLogListDto>>("AuditLog/GetAuditLogs", input);
        //}

        public async Task<PostDto> UpdateAsync(PostEditDto input)
        {
            return await httpClient.Post<PostDto>("post/Update", input);
        }
    }
}
