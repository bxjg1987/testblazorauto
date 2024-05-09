using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.Application.Share.Administrative;

namespace ZLJ.Admin.ClientProxy
{
    public class AdministrativeAppService :  IBXJGBaseInfoAdministrativeAppService
    {
        HttpClient _httpClient;
        public AdministrativeAppService(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateHttpClientAdmin();
        }




        public async Task<AdministrativeDto> CreateAsync(AdministrativeEditDto input)
        {
            return await _httpClient.Post<AdministrativeDto>("bxjgbaseinfoAdministrative/Create", input);
        }



        public async Task DeleteAsync(EntityDto<long> input)
        {
             await _httpClient.Post<BatchOperationOutputLong>("bxjgbaseinfoAdministrative/Delete", input);
        }

        public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            return await _httpClient.Post<BatchOperationOutputLong>("bxjgbaseinfoAdministrative/DeleteBatch", input);
        }

        public async Task<List<AdministrativeDto>> GetAllAsync(GetAdministrativeInput input)
        {
            return await _httpClient.Post<List<AdministrativeDto>>("bxjgbaseinfoAdministrative/getall", input);
        }


        public async Task<AdministrativeDto> GetAsync(EntityDto<long> input)
        {
            return await _httpClient.Post<AdministrativeDto>("bxjgbaseinfoAdministrative/Get", input);
        }

        public async Task<AdministrativeDto> MoveAsync(GeneralTreeNodeMoveInput input)
        {
            return await _httpClient.Post<AdministrativeDto>("bxjgbaseinfoAdministrative/Move", input);
        }

        public async Task<AdministrativeDto> UpdateAsync(AdministrativeEditDto input)
        {
            return await _httpClient.Post<AdministrativeDto>("bxjgbaseinfoAdministrative/Update", input);
        }
    }
}
