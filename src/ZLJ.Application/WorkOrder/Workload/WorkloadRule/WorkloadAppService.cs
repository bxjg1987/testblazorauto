using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization;
using ZLJ.App.Admin.WorkOrder.Workload.Dto;
using BXJG.Utils.Extensions;
using ZLJ.Configuration;
using Abp.Runtime.Session;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using Microsoft.EntityFrameworkCore;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.App.Admin.WorkOrder.Workload
{
    public class WorkloadAppService : AsyncCrudAppService<WorkloadRuleEntity, WorkloadDto, int, WorkloadGetAllInput, WorkloadCreateDto, WorkloadUpdateDto>
    {
        private readonly IRepository<StaffInfoEntity, long> _staffInfoRepository;
        public WorkloadAppService(IRepository<WorkloadRuleEntity> repository, IRepository<StaffInfoEntity, long> staffInfoRepository) : base(repository)
        {

            CreatePermissionName = PermissionNames.BXJGBaseInfoWorkloadRuleDefinitionCreate;
            UpdatePermissionName = PermissionNames.BXJGBaseInfoWorkloadRuleDefinitionUpdate;
            DeletePermissionName = PermissionNames.BXJGBaseInfoWorkloadRuleDefinitionDelete;
            GetPermissionName = PermissionNames.BXJGBaseInfoWorkloadRuleDefinition;
            _staffInfoRepository = staffInfoRepository;
        }

        public async Task<ListResultDto<WorkloadRuleListDto>> GetWorkloadRuleList(GetWorkloadRulesInput input)
        {
            //获取全部维修人员数据(岗位id为维修人员在种子数据配置死)
            var staffList = new List<StaffInfoEntity>();// [0];// await _staffInfoRepository.GetAll().Where(p => p.PostId == 24).ToListAsync();
            string WorkloadTypeStr = await SettingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadType, AbpSession.GetTenantId());
            var workloadType = (WorkloadType)Enum.Parse(typeof(WorkloadType), WorkloadTypeStr);
            var list = await Repository.GetAllListAsync(p => p.WorkloadRuleType == input.WorkloadRuleType && p.WorkloadType== workloadType);
            var dtos = base.ObjectMapper.Map<List<WorkloadRuleListDto>>(list);
            GetRuleParamsFormat(input.WorkloadRuleType, staffList, dtos);
            return new ListResultDto<WorkloadRuleListDto>(dtos);
        }
        private void GetRuleParamsFormat(WorkloadRuleType workloadRuleType,List<StaffInfoEntity> staffInfos, List<WorkloadRuleListDto> workloadRuleListDtos)
        {
            switch (workloadRuleType)
            {
                case WorkloadRuleType.ByWorkYears:
                    List<int> workYearList = new List<int>();
                    workYearList.Add(0);
                    List<WorkYearsInterval> workYearsIntervals = new List<WorkYearsInterval>();
                    foreach (var item in workloadRuleListDtos)
                    {

                        WorkYearsInterval workYearsInterval = new WorkYearsInterval();
                        workYearsInterval.Points = item.Points;
                        workYearsInterval.Max = Convert.ToInt32(item.RuleParams);
                        workYearsIntervals.Add(workYearsInterval);
                        workYearList.Add(workYearsInterval.Max);
                    }
                    workYearList.Sort();
                    workYearsIntervals = workYearsIntervals.OrderBy(p => p.Max).ToList();
                    for (int i = 0; i < workYearsIntervals.Count; i++)
                    {
                        workYearsIntervals[i].Min = workYearList[i];
                        workYearsIntervals[i].Max = workYearsIntervals[i].Max;
                    }
                    foreach (var item in workloadRuleListDtos)
                    {
                        foreach (var model in workYearsIntervals)
                        {
                            if (model.Max == Convert.ToInt32(item.RuleParams))
                            {
                                item.RuleParamsFormat = model.Min + "年~" + model.Max + "年";
                                break;
                            }
                        }
                    }
                    workloadRuleListDtos= workloadRuleListDtos.OrderBy(p => p.RuleParams).ToList();
                    break;
                case WorkloadRuleType.ByPersonal:
                    foreach (var staff in staffInfos)
                    {
                        foreach (var item in workloadRuleListDtos)
                        {
                            if (item.RuleParams.Contains(staff.Id.ToString()))
                            {
                                if (string.IsNullOrWhiteSpace(item.RuleParamsFormat))
                                {
                                    item.RuleParamsFormat = staff.Name;
                                }
                                else
                                {
                                    item.RuleParamsFormat = item.RuleParamsFormat+","+ staff.Name;
                                }
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }


        }
        public override async Task<WorkloadDto> CreateAsync(WorkloadCreateDto input)
        {
            string WorkloadTypeStr = await SettingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadType, AbpSession.GetTenantId());
            var workloadType = (WorkloadType)Enum.Parse(typeof(WorkloadType), WorkloadTypeStr);
            var existsQuery = Repository.GetAll().Where(x => x.WorkloadType == workloadType && x.WorkloadRuleType == input.WorkloadRuleType && x.RuleParams == input.RuleParams);
            if (await AsyncQueryableExecuter.AnyAsync(existsQuery))
                throw new UserFriendlyException("工作量规则已存在");
            return await base.CreateAsync(input);
        }

        public override async Task<WorkloadDto> UpdateAsync(WorkloadUpdateDto input)
        {
            string WorkloadTypeStr = await SettingManager.GetSettingValueForTenantAsync(AppSettingNames.TenantManagement.Workload.WorkloadType, AbpSession.GetTenantId());
            var workloadType = (WorkloadType)Enum.Parse(typeof(WorkloadType), WorkloadTypeStr);
            var existsQuery = Repository.GetAll().Where(x => x.WorkloadType == workloadType && x.WorkloadRuleType == input.WorkloadRuleType && x.RuleParams == input.RuleParams && input.Id != x.Id);
            if (await AsyncQueryableExecuter.AnyAsync(existsQuery))
                throw new UserFriendlyException("工作量规则已存在");
            var list = await Repository.GetAll().Where(x => x.WorkloadType == workloadType && x.WorkloadRuleType == input.WorkloadRuleType && input.Id != x.Id).ToListAsync();
            var inputIds=input.RuleParams.Split(',');
            bool isExist = false;
            foreach (var item in list)
            {
                var ids = item.RuleParams.Split(',');
                var existList = ids.Intersect(inputIds).ToList();
                if (existList.Count > 0)
                {
                    isExist = true;
                    break;
                }
            }
            if (isExist)
                throw new UserFriendlyException("当前选择的维修人员已在其他规则存在，请确认！");
            return await base.UpdateAsync(input);
        }

        public List<SeleteOption> GetWorkloadRuleTypeList()
        {
            
            List<SeleteOption> seleteOptions = new List<SeleteOption>();
            var list = Enum.GetValues<WorkloadRuleType>().ToList();
            foreach (var item in list)
            {
                SeleteOption seleteOption = new SeleteOption();
                seleteOption.Label = item.GetDescription();
                seleteOption.Value = Enum.GetName(item);
                seleteOption.Index = (int)item;
                seleteOptions.Add(seleteOption);
            }
            return seleteOptions;
        }

        public async Task<List<StaffInfoEntity>> GetStaffInfos()
        {
            return new List<StaffInfoEntity>();
            //获取全部维修人员数据(岗位id为维修人员在种子数据配置死)
            //var staffList = await _staffInfoRepository.GetAll().Where(p => p.PostId == 24).ToListAsync();
            //return staffList;
        }
    }
}
