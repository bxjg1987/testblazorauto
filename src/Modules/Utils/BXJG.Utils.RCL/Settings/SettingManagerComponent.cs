using BXJG.Utils.Application.Share.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.RCL.Settings
{
    /// <summary>
    /// 抽象的设置管理组件
    /// </summary>
    /// <typeparam name="TGroupDto">设置组dto</typeparam>
    /// <typeparam name="TDto">设置定义和值的dto</typeparam>
    /// <typeparam name="TEditDto">设置编辑dto</typeparam>
    public abstract class SettingManagerComponent<TGroupDto, TDto, TEditDto> : Components.BaseComponent
        where TGroupDto : SettingDefinitionGroupDto<TGroupDto>, new()
        where TDto : SettingDto<TGroupDto>
        where TEditDto : SettingEditDto,new()
    {
        /// <summary>
        /// 设置组数据源
        /// </summary>
        protected List<TGroupDto> DataSource = new List<TGroupDto>();
        /// <summary>
        /// 当前设置组
        /// </summary>
        protected TGroupDto? currGroup;
        /// <summary>
        /// 当前组下的设置列表
        /// </summary>
        protected List<TDto> CurrItems = new List<TDto>();
        //Tree<SettingDefinitionGroupDto> tree;
        // string? currKey;
        /// <summary>
        /// 默认选择的组名称
        /// </summary>
        protected string? defaultSelectedKey;
        // [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadData();
            //defaultSelectedKey = DataSource.FirstOrDefault()?.Name;
        }
        // [AbpExceptionInterceptor]
        /// <summary>
        /// 用户选择组变化时执行
        /// </summary>
        /// <param name="curr"></param>
        /// <returns></returns>
        protected async Task LoadCuree(TGroupDto curr)
        {
            if (curr != currGroup)
            {
                currGroup = curr;
                await LoadCurrData();
                // currGroup = DataSource.FirstOrDefault();
                //  defaultSelectedKey = currGroup?.Name;
                // await LoadCurrData();
            }
        }
        /// <summary>
        /// 当前页面是否忙碌中
        /// </summary>
        protected virtual bool isBusy => isLoading || isSaving;
        /// <summary>
        /// 是否正在加载数据
        /// </summary>
        protected bool isLoading = false;
        /// <summary>
        /// 加载组列表
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadData()
        {
            if (isLoading)
                return;

            isLoading = true;
            try
            {   ///api/services/app/Settings/GetAllGroups
                DataSource = await HttpClient.Post<List<TGroupDto>>("Settings", action: "GetAllGroups", default);
            }
            finally
            {
                isLoading = false;
            }

            if (!DataSource.Any(d => d.Name == currGroup?.Name))
            {
                currGroup = DataSource.FirstOrDefault();
                defaultSelectedKey = currGroup?.Name;
                await LoadCurrData();
            }
        }
        /// <summary>
        /// 加载当前组下的设置
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadCurrData()
        {
            if (isLoading)
                return;

            isLoading = true;
            try
            {
                CurrItems = await HttpClient.Post<List<TDto>>("Settings", action: "GetSettingByGroup", new { id = currGroup.Name }, default);
            }
            finally
            {
                isLoading = false;
            }
        }
        /// <summary>
        /// 是否正在保存
        /// </summary>
        bool isSaving = false;
        /// <summary>
        /// 保存当前组下的所有设置
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Save()
        {
            if (isSaving)
                return;

            isSaving = true;
            try
            {
                await HttpClient.Post("Settings", action: "Update", CurrItems.Select(x => new TEditDto
                {
                    Name = x.Name,
                    Value = x.Value
                }), default);
                _ = ShowSuccessMessage("保存提示");
            }
            finally
            {
                isSaving = false;
            }
        }
    }
    /// <summary>
    /// 默认的设置管理组件
    /// </summary>
    public class SettingManagerComponent : SettingManagerComponent<SettingDefinitionGroupDto,
        SettingDto,
        SettingEditDto>
    {
    }
}
