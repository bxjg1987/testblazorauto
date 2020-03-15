using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Asset
{
    public interface IEquipmentInfoAppService 
        : IAsyncCrudAppService<EquipmentInfoDto,
                               long,
                               GetEquipmentInfoInput,
                               EquipmentInfoEditDto, 
                               EquipmentInfoEditDto, 
                               EntityDto<long>,
                               EntityDto<long>>

    {
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<long>> DeleteBatchAsync(long[] input);
        /// <summary>
        /// 获取附近的设备
        /// </summary>
        /// <param name="latitude">当前纬度</param>
        /// <param name="longitude">当前经度</param>
        /// <param name="distance">搜索多少米以内的数据</param>
        /// <returns></returns>
        public Task<List<EquipmentInfoDto>> GetEnclosureAsync(double latitude, double longitude, double distance);
    }
}
