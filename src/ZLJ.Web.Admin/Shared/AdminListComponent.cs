using Abp.Application.Services.Dto;
using Abp.Localization.Sources;
using BXJG.Common;
using BXJG.MudBlazor.Components;
using BXJG.Utils;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.Shared
{
    /// <summary>
    /// 后台管理 crud中的列表页
    /// </summary>
    /// <typeparam name="TAppService">abp应用服务类型</typeparam>
    /// <typeparam name="TFormDialogCoponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数类型</typeparam>
    public class AdminListComponent<TAppService,
                                    TFormDialogCoponent,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput,
                                    TUpdateInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                  TEntityDto,
                                                                                  TPrimaryKey,
                                                                                  TGetAllInput,
                                                                                  TCreateInput,
                                                                                  TUpdateInput>
        where TFormDialogCoponent : ComponentBase
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        #region 本地化
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
        /// <summary>
        /// 获取App.Common中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != ZLJ.App.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(ZLJ.App.Common.Consts.Common);
                }

                return appCommonLocalizationSource;
            }
        }
        /// <summary>
        /// 获取ZLJ.Core中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceAppZLJ
        {
            get
            {

                if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJConsts.LocalizationSourceName)
                {
                    zljLocalizationSource = LocalizationManager.GetSource(ZLJConsts.LocalizationSourceName);
                }

                return zljLocalizationSource;
            }
        }
        /// <summary>
        /// 获取BXJG.Utils中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceUtils
        {
            get
            {

                if (utilsLocalizationSource == null || utilsLocalizationSource.Name != BXJGUtilsConsts.LocalizationSourceName)
                {
                    utilsLocalizationSource = LocalizationManager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
                }

                return utilsLocalizationSource;
            }
        }
        /// <summary>
        /// 获取App.Admin中的本地化源
        /// </summary>
        protected override string LocalizationSourceName => ZLJ.App.Admin.AdminConsts.Admin;
        #endregion

        #region 弹窗
        /// <summary>
        /// 新增时的弹窗选项对象
        /// </summary>
        protected virtual DialogOptions DialogAddOptions => new DialogOptions { CloseOnEscapeKey = true };
        /// <summary>
        /// 修改时的弹窗选项对象
        /// </summary>
        protected virtual DialogOptions DialogEditOptions => DialogAddOptions;
        /// <summary>
        /// 弹窗服务
        /// </summary>
        [Inject]
        protected virtual IDialogService? DialogService { get; private set; }//用ScopeServiceProvider不行，试过了
        /// <summary>
        /// 准备新增时传入弹窗的参数
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask<DialogParameters<TFormDialogCoponent>> GetAddParams()
        {
            var ps = new DialogParameters<TFormDialogCoponent>
            {
                { "Pattern", FrmPattern.Add },
            };
            return ValueTask.FromResult(ps);
        }
        /// <summary>
        /// 准备修改时传入弹窗的参数
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask<DialogParameters<TFormDialogCoponent>> GetEditParams()
        {
            var ps = new DialogParameters<TFormDialogCoponent>
            {
                { "Pattern", FrmPattern.Edit },
                { "Model", dataGrid.SelectedItem }
            };
            return ValueTask.FromResult(ps);
        }
        /// <summary>
        /// 点击新增按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task AddClick()
        {
            await base.SafelyExecuteAsync(async delegate
            {
                if (!(await DialogService.Show<TFormDialogCoponent>("新增" + FuncName, await GetAddParams(), DialogAddOptions).Result).Canceled)
                {
                    await dataGrid.ReloadServerData();
                }
            });
        }
        /// <summary>
        /// 点击修改按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task EditClick()
        {
            //没有选择数据时，修改按钮是禁用的，所以没必要再判断一次
            await base.SafelyExecuteAsync(async delegate
            {
                if (!(await DialogService.Show<TFormDialogCoponent>("修改" + FuncName, await GetEditParams(), DialogEditOptions).Result).Canceled)
                {
                    await dataGrid.ReloadServerData();
                }
            });
        }
        #endregion
    }
    /// <summary>
    /// 后台管理 crud中的列表页
    /// </summary>
    /// <typeparam name="TAppService">abp应用服务类型</typeparam>
    /// <typeparam name="TFormDialogCoponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数类型</typeparam>
    /// 
    public class AdminListComponent<TAppService,
                                    TFormDialogCoponent,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput> : AdminListComponent<TAppService,
                                                                       TFormDialogCoponent,
                                                                       TEntityDto,
                                                                       TPrimaryKey,
                                                                       TGetAllInput,
                                                                       TCreateInput,
                                                                       TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TFormDialogCoponent : ComponentBase
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    { }
    /// <summary>
    /// 后台管理 crud中的列表页
    /// </summary>
    /// <typeparam name="TAppService">abp应用服务类型</typeparam>
    /// <typeparam name="TFormDialogCoponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时输入参数类型</typeparam>
    /// 
    public class AdminListComponent<TAppService,
                                    TFormDialogCoponent,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput> : AdminListComponent<TAppService,
                                                                       TFormDialogCoponent,
                                                                       TEntityDto,
                                                                       TPrimaryKey,
                                                                       TGetAllInput,
                                                                       TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TFormDialogCoponent : ComponentBase
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    { }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TAppService">abp应用服务类型</typeparam>
    /// <typeparam name="TFormDialogCoponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class AdminListComponent<TAppService,
                                    TFormDialogCoponent,
                                    TEntityDto,
                                    TPrimaryKey> : AdminListComponent<TAppService,
                                                                      TFormDialogCoponent,
                                                                      TEntityDto,
                                                                      TPrimaryKey,
                                                                      PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TFormDialogCoponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TAppService">abp应用服务类型</typeparam>
    /// <typeparam name="TFormDialogCoponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    public class AdminListComponent<TAppService,
                                    TFormDialogCoponent,
                                    TEntityDto> : AdminListComponent<TAppService,
                                                                     TFormDialogCoponent,
                                                                     TEntityDto,
                                                                     int>
        where TEntityDto : IEntityDto<int>
        where TFormDialogCoponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}