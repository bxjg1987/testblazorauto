using Abp.Application.Services.Dto;
using Abp.Localization.Sources;
using BXJG.AbpMudBlazor.Components;
using BXJG.Common;
using BXJG.AbpMudBlazor.Components;
using BXJG.Utils;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.Shared
{
    ///// <summary>
    ///// 后台管理 crud中的列表页
    ///// </summary>
    ///// <typeparam name="TAppService">abp应用服务类型</typeparam>
    ///// <typeparam name="TEntityDto">详情对象类型</typeparam>
    ///// <typeparam name="TPrimaryKey">主键类型</typeparam>
    ///// <typeparam name="TGetAllInput">获取列表时输入参数类型</typeparam>
    ///// <typeparam name="TCreateInput">新增时输入参数类型</typeparam>
    ///// <typeparam name="TUpdateInput">修改时输入参数类型</typeparam>
    //public abstract class AdminListBaseComponent<TAppService,
    //                                             TEntityDto,
    //                                             TPrimaryKey,
    //                                             TGetAllInput,
    //                                             TCreateInput,
    //                                             TUpdateInput> : AbpMudListBaseComponent<TAppService,
    //                                                                                     TEntityDto,
    //                                                                                     TPrimaryKey,
    //                                                                                     TGetAllInput,
    //                                                                                     TCreateInput,
    //                                                                                     TUpdateInput>
    //    where TEntityDto : IEntityDto<TPrimaryKey>
    //    where TUpdateInput : IEntityDto<TPrimaryKey>
    //    where TGetAllInput : new()
    //    where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    //{
    //    #region 本地化
    //    private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
    //    /// <summary>
    //    /// 获取App.Common中的本地化源
    //    /// </summary>
    //    protected virtual ILocalizationSource LocalizationSourceAppCommon
    //    {
    //        get
    //        {
    //            if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != ZLJ.App.Common.Consts.Common)
    //            {
    //                appCommonLocalizationSource = LocalizationManager.GetSource(ZLJ.App.Common.Consts.Common);
    //            }

    //            return appCommonLocalizationSource;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取ZLJ.Core中的本地化源
    //    /// </summary>
    //    protected virtual ILocalizationSource LocalizationSourceAppZLJ
    //    {
    //        get
    //        {

    //            if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJConsts.LocalizationSourceName)
    //            {
    //                zljLocalizationSource = LocalizationManager.GetSource(ZLJConsts.LocalizationSourceName);
    //            }

    //            return zljLocalizationSource;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取BXJG.Utils中的本地化源
    //    /// </summary>
    //    protected virtual ILocalizationSource LocalizationSourceUtils
    //    {
    //        get
    //        {
    //            if (utilsLocalizationSource == null || utilsLocalizationSource.Name != BXJGUtilsConsts.LocalizationSourceName)
    //            {
    //                utilsLocalizationSource = LocalizationManager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
    //            }

    //            return utilsLocalizationSource;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取App.Admin中的本地化源
    //    /// </summary>
    //    protected override string LocalizationSourceName => ZLJ.App.Admin.AdminConsts.Admin;
    //    #endregion
    //}

    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// 使用弹窗弹出新增和详情窗口
    /// </summary>
    /// <typeparam name="TCreateDialog">弹窗组件类型</typeparam>
    /// <typeparam name="TDetailDialog">弹窗组件类型</typeparam>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AdminListBaseComponent<TCreateDialog,
                                                       TDetailDialog,
                                                       TAppService,
                                                       TEntityDto,
                                                       TPrimaryKey,
                                                       TGetAllInput,
                                                       TCreateInput,
                                                       TUpdateInput> : AbpMudListDialogBaseComponent<TCreateDialog,
                                                                                                     TDetailDialog,
                                                                                                     TAppService,
                                                                                                     TEntityDto,
                                                                                                     TPrimaryKey,
                                                                                                     TGetAllInput,
                                                                                                     TCreateInput,
                                                                                                     TUpdateInput>
        where TCreateDialog : ComponentBase
        where TDetailDialog : ComponentBase
        where TEntityDto : IEntityDto<TPrimaryKey> , IExtendableDto
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
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
    }
}