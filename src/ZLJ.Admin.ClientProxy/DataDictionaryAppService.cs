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
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Common.Contracts;

namespace ZLJ.Admin.ClientProxy
{
    public class DataDictionaryAppService : IDataDictionaryAppService
    {
        HttpClient _httpClient;
        public DataDictionaryAppService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateHttpClientUtils();
        }

     

      
        public async Task<DataDictionaryDto> CreateAsync(DataDictionaryEditDto input)
        {
            return await _httpClient.Post<DataDictionaryDto>("DataDictionary/Create", input);
        }

    

        public async Task DeleteAsync(EntityDto<long> input)
        {
            await _httpClient.Post<BatchOperationOutputLong>("DataDictionary/Delete", input);
        }

        public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            return await _httpClient.Post<BatchOperationOutputLong>("DataDictionary/ DeleteBatch", input);
        }

        public async Task<List<DataDictionaryDto>> GetAllAsync(DataDictionaryGetTreeInput input)
        {
            return await _httpClient.Post<List<DataDictionaryDto>>("DataDictionary/getall", input);
        }


        public async Task<DataDictionaryDto> GetAsync(EntityDto<long> input)
        {
            return await _httpClient.Post<DataDictionaryDto>("DataDictionary/Get", input);
        }

        public async Task<DataDictionaryDto> MoveAsync(GeneralTreeNodeMoveInput input)
        {
            return await _httpClient.Post<DataDictionaryDto>("DataDictionary/Move", input);
        }

        public async Task<DataDictionaryDto> UpdateAsync(DataDictionaryEditDto input)
        {
            return await _httpClient.Post<DataDictionaryDto>("DataDictionary/Update", input);
        }
    }
}
