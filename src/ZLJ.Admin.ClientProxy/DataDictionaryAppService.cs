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
using BXJG.Utils.Application.Share.GeneralTree;

namespace ZLJ.Admin.ClientProxy
{
    public class DataDictionaryAppService : BaseAppServiceClient,IDataDictionaryAppService
    {
        public DataDictionaryAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

     

      
        public async Task<DataDictionaryDto> CreateAsync(DataDictionaryEditDto input)
        {
            return await Post<DataDictionaryDto>("api/services/bxjgutils/DataDictionary/Create", input);
        }

    

        public async Task<BatchOperationOutputLong> DeleteAsync(BatchOperationInputLong input)
        {
          return  await Post<BatchOperationOutputLong>("api/services/bxjgutils/DataDictionary/Delete", input);
        }

       

        public async Task<List<DataDictionaryDto>> GetAllAsync(DataDictionaryGetTreeInput input)
        {
            return await Post<List<DataDictionaryDto>>("api/services/bxjgutils/DataDictionary/getall", input);
        }


        public async Task<DataDictionaryDto> GetAsync(EntityDto<long> input)
        {
            return await Post<DataDictionaryDto>("api/services/bxjgutils/DataDictionary/Get", input);
        }

        public async Task<DataDictionaryDto> MoveAsync(GeneralTreeNodeMoveInput input)
        {
            return await Post<DataDictionaryDto>("api/services/bxjgutils/DataDictionary/Move", input);
        }

        public async Task<DataDictionaryDto> UpdateAsync(DataDictionaryEditDto input)
        {
            return await Post<DataDictionaryDto>("api/services/bxjgutils/DataDictionary/Update", input);
        }
    }
}
