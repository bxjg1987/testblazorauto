using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;
using ZLJ.Configuration;
using ZLJ.WorkOrder.Workload.WorkloadRuleHandler;

namespace ZLJ.WorkOrder.Workload
{
    public class WorkloadManage : BXJGBaseInfoDomainServiceBase, IWorkloadManage
    {
        public IAbpSession AbpSession { get; set; }
        private readonly ISettingManager _settingManager;
        private readonly IRepository<StaffInfoEntity, long> _staffInfoRepository;
        private readonly IRepository<WorkloadRuleEntity> _workloadRuleRepository;
        private readonly IRepository<WorkloadRecordEntity, Guid> _workloadRecordRepository;
        public WorkloadManage(ISettingManager settingManager
            , IRepository<StaffInfoEntity, long> staffInfoRepository
            , IRepository<WorkloadRuleEntity> workloadRuleRepository
            , IRepository<WorkloadRecordEntity, Guid> workloadRecordRepository)
        {
            AbpSession = NullAbpSession.Instance;
            _settingManager = settingManager;
            _staffInfoRepository = staffInfoRepository;
            _workloadRuleRepository = workloadRuleRepository;
            _workloadRecordRepository = workloadRecordRepository;
        }
        //1.获取全局配置的工作量设置和工作量规则设置
        //2.获取全部维修人员数据(todo:员工编辑界面如果岗位是维修人员则可以选择设置 工作量规则)
        //3.遍历维修人员数据,首先判断是否有配置工作量规则，有则按此工作量规则(后期实现)，没有则按全局工作量规则
        //4.根据全局的工作量设置和相应的工作量规则从数据库获取工作量规则配置数据进行处理，然后找到符合此员工的工作量规则配置的积分
        //5.生成此员工这个月的工作量统计记录
        public async Task CreateWorkloadRecordMonthly()
        {

            //获取全部维修人员的工作量积分
            var list = await GetAllStaffWorkloadPoints();
            //生成此员工这个月的工作量统计记录
            foreach (var item in list)
            {
                WorkloadRecordEntity entity = new WorkloadRecordEntity();
                entity.StatisticsTime = Clock.Now.AddDays(1 - DateTime.Now.Day).Date;
                entity.RulePoints = item.Points;
                entity.ActualPoints = 0;
                entity.StaffInfoId = item.StaffId;
                var model=await _workloadRecordRepository.FirstOrDefaultAsync(p => p.StaffInfoId == item.StaffId && p.StatisticsTime == entity.StatisticsTime);
                if (model == null)
                {
                    await _workloadRecordRepository.InsertAsync(entity);
                }
                else
                {
                    model.RulePoints = item.Points;
                    model.ActualPoints = 0;
                    await _workloadRecordRepository.UpdateAsync(model);
                }
            }            
        }

        private async Task<IReadOnlyList<StaffWorkloadPointsDto>> GetAllStaffWorkloadPoints()
        {
            List<StaffWorkloadPointsDto> list = new List<StaffWorkloadPointsDto>();

            //获取设置的工作量类型，工作量规则类型以及默认的工作量积分
            string WorkloadTypeStr = await _settingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadType, AbpSession.GetTenantId());
            var workloadType =(WorkloadType)Enum.Parse(typeof(WorkloadType), WorkloadTypeStr);
            string WorkloadRuleTypeStr = await _settingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadRuleType, AbpSession.GetTenantId());
            var workloadRuleType = (WorkloadRuleType)Enum.Parse(typeof(WorkloadRuleType), WorkloadRuleTypeStr);
            int globalWorkloadPoints = Convert.ToInt32(await _settingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadPoints, AbpSession.GetTenantId()));

            //获取全部的工作量规则数据
            List<WorkloadRuleEntity> allWorkloadRuleEntities = await _workloadRuleRepository.GetAllListAsync();

            //获取符合全局设置的工作量规则类型的工作量规则数据
            List<WorkloadRuleEntity> globalWorkloadRuleEntities = allWorkloadRuleEntities.Where(p => p.WorkloadType == workloadType && p.WorkloadRuleType == workloadRuleType).ToList();

            //获取全部维修人员数据(岗位id为维修人员在种子数据配置死)
            var staffList = new StaffInfoEntity[0]; //await _staffInfoRepository.GetAll().Where(p => p.PostId == 24).ToListAsync();  //todo 同时获取其单独设置的工作量积分
            foreach (var item in staffList)
            {
                List<WorkloadRuleEntity> workloadRuleEntitiesStaff = new List<WorkloadRuleEntity>();
                //获取维修人员对应的工作量规则数据
                //todo 如果设置了维修员工的工作量规则类型，则按设置的走，否则走全局设置的工作量规则
                ///workloadRuleEntitiesStaff= allWorkloadRuleEntities.Where(p => p.WorkloadType == workloadType && p.WorkloadRuleType == item.WorkloadRuleType).ToList();
                workloadRuleEntitiesStaff = globalWorkloadRuleEntities;

                //获取单个维修人员的工作量积分
                StaffWorkloadPointsDto staffWorkloadPointsDto = new StaffWorkloadPointsDto();
                staffWorkloadPointsDto.StaffId = item.Id;
                staffWorkloadPointsDto.Points = GetStaffWorkloadPoints(workloadRuleType, globalWorkloadPoints, item, workloadRuleEntitiesStaff);
                list.Add(staffWorkloadPointsDto);
            }

            return list;
        }
        private int GetStaffWorkloadPoints(WorkloadRuleType workloadRuleType, int globalWorkloadPoints, StaffInfoEntity staffInfo, List<WorkloadRuleEntity> workloadRuleEntities)
        {
            int points;
            IWorkloadRuleHandler workloadRuleHandler;
            switch (workloadRuleType)
            {
                case WorkloadRuleType.ByWorkYears:
                    workloadRuleHandler = new ByWorkYearsHandler();
                    break;
                case WorkloadRuleType.ByPersonal:
                    workloadRuleHandler =new ByPersonalHandler();
                    break;
                default:
                    workloadRuleHandler = new ByWorkYearsHandler();
                    break;
            }
            points = workloadRuleHandler.GetStaffWorkloadPoints(staffInfo, globalWorkloadPoints, workloadRuleEntities);
            return points;
        }
    }
    public class WorkYearsInterval
    {
        public int Points { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
    }
}
