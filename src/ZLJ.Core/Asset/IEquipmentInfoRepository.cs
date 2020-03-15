using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Asset
{
    public interface IEquipmentInfoRepository: IRepository<EquipmentInfoEntity, long>
    {
        /// <summary>
        /// 获取附近的设备
        /// </summary>
        /// <param name="latitude">当前纬度</param>
        /// <param name="longitude">当前经度</param>
        /// <param name="distance">搜索多少米以内的数据</param>
        /// <returns></returns>
        public Task<List<EquipmentInfoEntity>> GetEnclosureAsync(double latitude, double longitude, double distance);
    }
}
