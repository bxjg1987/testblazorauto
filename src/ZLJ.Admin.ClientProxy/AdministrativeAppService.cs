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
    public class AdministrativeAppService : BaseAppServiceClient, IBXJGBaseInfoAdministrativeAppService
    {
        public AdministrativeAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }




        public async Task<AdministrativeDto> CreateAsync(AdministrativeEditDto input)
        {
            return await Post<AdministrativeDto>("api/services/app/bxjgbaseinfoAdministrative/Create", input);
        }



        public async Task<BatchOperationOutputLong> DeleteAsync(BatchOperationInputLong input)
        {
            return await Post<BatchOperationOutputLong>("api/services/app/bxjgbaseinfoAdministrative/Delete", input);
        }



        public async Task<List<AdministrativeDto>> GetAllAsync(GetAdministrativeInput input)
        {
            return await Post<List<AdministrativeDto>>("api/services/app/bxjgbaseinfoAdministrative/getall", input);
        }


        public async Task<AdministrativeDto> GetAsync(EntityDto<long> input)
        {
            return await Post<AdministrativeDto>("api/services/app/bxjgbaseinfoAdministrative/Get", input);
        }

        public async Task<AdministrativeDto> MoveAsync(GeneralTreeNodeMoveInput input)
        {
            return await Post<AdministrativeDto>("api/services/app/bxjgbaseinfoAdministrative/Move", input);
        }

        public async Task<AdministrativeDto> UpdateAsync(AdministrativeEditDto input)
        {
            return await Post<AdministrativeDto>("api/services/app/bxjgbaseinfoAdministrative/Update", input);
        }
    }
}
