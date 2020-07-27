using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BXJG.Common.Dto;

namespace BXJG.Equipment.EquipmentInfo
{
    public class BXJGEquipmentEquipmentInfoAppService : AsyncCrudAppService<EquipmentInfoEntity, EquipmentInfoDto, long, EquipmentInfoGetAllInput, EquipmentInfoEditDto>, IBXJGEquipmentEquipmentInfoAppService
    {
        public BXJGEquipmentEquipmentInfoAppService(IRepository<EquipmentInfoEntity, long> repository) : base(repository)
        {
        }

        public async Task<BatchOperationResultLong> BatchDeleteAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationResultLong();
            foreach (var item in input.Ids)
            {
                try
                {
                    await base.Repository.DeleteAsync(item);
                    result.Ids.Add(item);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn($"删除设备档案失败，设备Id：{item}", ex);
                }
            }
            return result;
        }

        protected override IQueryable<EquipmentInfoEntity> CreateFilteredQuery(EquipmentInfoGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .Include(c => c.Area)
                .WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.Area.Code.StartsWith(input.AreaCode))
                .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keywords) ||
                                                                    c.HardwareCode.Contains(input.Keywords));
        }
    }
}
