using Abp.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using txdl;
using ZLJ.Equipments.EquipmentInstances;
using Microsoft.AspNetCore.Mvc;
using Abp.Auditing;
using ZLJ.BaseInfo.EquipmentInfo;
using ZLJ.Customer;

namespace ZLJ.App.txdl
{
    //注意所有设备id地方 要使用设备序列号，不用使用租赁但明细id

    /// <summary>
    /// 提供设备配置信息的接口
    /// </summary>
    [DisableAuditing]
    [UnitOfWork(false)]
    public class TXDLEquipmentInfoProviderAppService : ApplicationService
    {
        private readonly Lazy<IRepository<EquipmentInstanceEntity, string>> equipmentInstanceRepository;
        private readonly Lazy<IRepository<Rent.Order.OrderEntity, long>> rentOrderRepository;
        private readonly Lazy<IRepository<CustomerStaffInfoEntity, long>> customrStaffRepository;
        //private readonly ComunicationParameterMetaDataManager comunicationParameterMetaDataManager;

        public TXDLEquipmentInfoProviderAppService(Lazy<IRepository<EquipmentInstanceEntity, string>> equipmentInstanceRepository,
                                                   //ComunicationParameterMetaDataManager comunicationParameterMetaDataManager,
                                                   Lazy<IRepository<Rent.Order.OrderEntity, long>> rentOrderRepository,
                                                   Lazy<IRepository<CustomerStaffInfoEntity, long>> customrStaffRepository)
        {
            this.equipmentInstanceRepository = equipmentInstanceRepository;
            //this.comunicationParameterMetaDataManager = comunicationParameterMetaDataManager;
            this.rentOrderRepository = rentOrderRepository;
            this.customrStaffRepository = customrStaffRepository;
        }

        /// <summary>
        /// 获取所有设备实例的配置信息
        /// </summary>
        [HttpPost]
        public async Task<IEnumerable<EquipmentConfig>> GetEquipmentConfigsByIdsAsync(IEnumerable<string> machineNos)
        {
            this.DisableTenantFilter();
            IQueryable<EquipmentInstanceEntity> q = equipmentInstanceRepository.Value.GetAll()
                                                                                     .AsNoTrackingWithIdentityResolution()
                                                                                     .Include(c => c.EquipmentInfo)
                                                                                     .Where(c => c.LifeCycleStatus == LifeCycleStatus.Normal);

            if (machineNos != null && machineNos.Count() > 0)
                q = q.Where(c => machineNos.Contains(c.Id));

            var tempList = await q.ToListAsync();
            //tempList.ForEach(e => {
            //    e.CommunicationParameterMetaData = this.comunicationParameterMetaDataManager[e.CommunicationType];
            //});
            var query = tempList.Select(entity => new EquipmentConfig
            {
                MachineNo = entity.Id,
                CommunicationType = entity.CommunicationType,
                CommunicationParameters = entity.GetCommunicationParameters(),
                Name = entity.EquipmentInfo.Name + entity.Id,
                RowVersion = entity.CommunicationParameters
            });
            return query.ToList();
        }

        /// <summary>
        /// 获取指定设备的配置信息
        /// </summary>
        public async Task<IEnumerable<InfoVersion>> GetEquipmentConfigStatusAsync()
        {
            this.DisableTenantFilter();
            var q = equipmentInstanceRepository.Value.GetAll()
                                                     .AsNoTrackingWithIdentityResolution()
                                                     .Where(c => c.LifeCycleStatus == LifeCycleStatus.Normal)
                                                     .Select(c => new InfoVersion
                                                     {
                                                         Id = c.Id,
                                                         RowVersion = c.CommunicationParameters
                                                     });
            return await q.ToListAsync();
        }

        //private EquipmentConfig MapToEquipmentConfig(EquipmentInstanceEntity entity)
        //{
        //    return new EquipmentConfig
        //    {
        //        MachineNo = entity.MachineNo,
        //        CommunicationType = entity.EquipmentInfo.CommunicationType,
        //        CommunicationParameters = entity.GetCommunicationParameters(),
        //        Name = $"{entity.Id}-{entity.MachineNo}",
        //    };
        //}

        //private IQueryable<EquipmentInstanceEntity> CreateFilteredQuery()
        //{
        //    var query = _equipmentInstanceRepository.GetAll()
        //        .AsNoTrackingWithIdentityResolution()
        //        .Include(x => x.EquipmentInfo);
        //    return query;
        //}

        [Obsolete("直接使用InfoVersion.NullList")]
        private static readonly IEnumerable<InfoVersion> nullList = new List<InfoVersion>();

        [UnitOfWork(false)] 
        public async Task<IEnumerable<InfoVersion>> GetStaffIdVersion(string machineNo)
        {
            this.DisableTenantFilter();

            //根据设备id 去租赁明细里找得到客户id
            var customerId = await rentOrderRepository.Value.GetAll()
                                                            //.AsNoTrackingWithIdentityResolution() 就查个id，没必要加这个
                                                            .Where(c => c.Items.Any(d => d.Status == Rent.Order.DeliveryStatus.Renting && d.EquipmentInstanceId == machineNo))
                                                            .Select(c => c.AssociatedCompanyId)
                                                            .SingleOrDefaultAsync();

            if (customerId == default)
                return nullList; 

            //根据客户id获取员工列表
            var users = await customrStaffRepository.Value.GetAll()
                                                          //.AsNoTrackingWithIdentityResolution() //后面有映射，加这个没意义
                                                          .Where(c => /*c.IsActive &&*/ c.CustomerId == customerId)   //禁用的客户是允许的，因为他随时可能被启用，所以他的业务功能还是保留起 禁用的客户只是不能做新的业务
                                                          .Select(c => new InfoVersion
                                                          {
                                                              Id = c.Id.ToString(), //19位，老机器最大32个字符
                                                              RowVersion = c.ConcurrencyStamp.Substring(0,8),//机器那边的长度有限制，这里少取点 万一重复了，关系也不大 记得与GetStaffs保持一致
                                                          })
                                                          .ToListAsync();
            return users;
        }

        [UnitOfWork(false)]
        [HttpPost]
        public async Task<IEnumerable<EquipmentUser>> GetStaffs(IEnumerable<long> staffIds)
        {
            this.DisableTenantFilter();
            var users = await customrStaffRepository.Value.GetAll()
                                                          //.AsNoTrackingWithIdentityResolution() 下面有投影，所以这里没必要
                                                          .Where(c => staffIds.Contains(c.Id))
                                                          .Select(c => new EquipmentUser
                                                          {
                                                              DisplayName = c.Name,
                                                              Id = c.Id.ToString(),
                                                              LoginName = c.UserName,
                                                              Password = c.EquipmentPwd,
                                                              RowVersion = c.ConcurrencyStamp.Substring(0,8)//记得与GetStaffs保持一致
                                                          })
                                                          .ToListAsync();
            return users;
        }
    }
}