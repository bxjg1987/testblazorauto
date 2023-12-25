 using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using ZLJ.Equipments.EquipmentInstances;

namespace ZLJ.txdl
{
    public class PushEquipmentConfigChangeJob : IAsyncEventHandler<EntityCreatedEventData<EquipmentInstanceEntity>>,
                                                IAsyncEventHandler<EntityUpdatedEventData<EquipmentInstanceEntity>>,
                                                IAsyncEventHandler<EntityDeletedEventData<EquipmentInstanceEntity>>,
                                                ITransientDependency
    {
        private readonly EquipmentController _equipmentController;

        public PushEquipmentConfigChangeJob(EquipmentController equipmentController)
        {
            _equipmentController = equipmentController;
        }

        //public async Task ExecuteAsync(string[] args)
        //{
        //    //推送设备修改信息
        //    await _equipmentController.ConfigChangedAsync();
        //}

        public Task HandleEventAsync(EntityCreatedEventData<EquipmentInstanceEntity> eventData)
        {
            return _equipmentController.ConfigChangedAsync();
        }

        public Task HandleEventAsync(EntityUpdatedEventData<EquipmentInstanceEntity> eventData)
        {
            return _equipmentController.ConfigChangedAsync();
        }

        public Task HandleEventAsync(EntityDeletedEventData<EquipmentInstanceEntity> eventData)
        {
            return _equipmentController.ConfigChangedAsync();
        }
    }
}