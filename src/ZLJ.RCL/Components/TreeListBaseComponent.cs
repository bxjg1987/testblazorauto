using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using AntDesign;
using AntDesign.TableModels;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.GeneralTree;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZLJ.RCL.Interceptors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZLJ.RCL.Components
{
    /*
     * 之前我们实现过动态条件，参考BXJG.MudBlazor中的实现
     * 动态条件对于用户来讲有点复杂，所以我们暂时不考虑
     */

    /// <summary>
    /// 抽象的，基于MudBlazor treeView的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public abstract class TreeListBaseComponent<TAppService,
                                                TEntityDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput> : BXJG.Utils.RCL.Components.TreeListBaseComponent<TAppService,
                                                TEntityDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput>
        //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableObj//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
        //界面部分就不要用IPermissionChecker了，不过server模式时AuthorizationService内部会使用IPermissionChecker
        //请查看自定义授权策略提供器
        //客户端部分是直接在前端内存中比对的，有区别于server模式的，自定义的授权策略提供器

#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override void BtnClearFilterClick()
        {
            base.BtnClearFilterClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnDeleteClick()
        {
           await base.BtnDeleteClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnDeleteItemClick(TEntityDto item)
        {
          await  base.BtnDeleteItemClick(item);
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override void BtnRefreshClick()
        {
            base.BtnRefreshClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override void BtnSearchClick()
        {
            base.BtnSearchClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task LoadCore()
        {
            await base.LoadCore();
        }


        /// <summary>
        /// 对ant表格的引用
        /// </summary>
        protected Table<TEntityDto> table;

        /// <summary>
        /// 将ant table条件转换为TGetAllInput后加载数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual void OnQuery(QueryModel condition)
        {
            /*
             * 目前只考虑高级搜索方式，不考虑动态条件
             */



            var ls = condition.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority).Select(c => c.FieldName + " " + c.Sort.Replace("end", ""));
            Sorting = string.Join(",", ls);
            //页码和页索引直接在table做bingd-xxx
            //但若在这里做，则子类无需再绑定了
            //  this.PageSize = condition.PageSize;
            //  this.PageIndex = condition.PageIndex;
            // var r =await AppService.GetAllAsync(GetAllInput);
            //Items = r.Items;
            // TotalCount = r.TotalCount;
            //if (condition.SortList != null && condition.SortList.Count > 0)
            //    sd222.Sorting = string.Join(",", condition.SortList);
            //else if (condition.SortOrder != SortOrder.Unset)
            //    sd222.Sorting = condition.SortName + " " + condition.SortOrder.ToString();
            //else
            //    sd222.Sorting = "Id";

            //#region 关键字
            //IHaveKeywords gjz = null;
            //if (GetAllInput is IHaveKeywords cd4)
            //{
            //    gjz = cd4;
            //}
            //else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    gjz = cddqq;
            //}
            //gjz.Keywords = condition.SearchText;
            //#endregion

            ////动态条件的填充请已在父类中定义

             LoadListData();

            //return new QueryData<TEntityDto>
            //{
            //    IsAdvanceSearch = true,
            //    IsFiltered = true,
            //    IsSearch = true,
            //    IsSorted = true,
            //    Items = Items,
            //    TotalCount = base.TotalCount
            //};
        }
        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //
        [Inject]
        public IMessageService MessageService { get; set; }

        protected override void ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            MessageService.Error(msg);
            StateHasChanged();
            Thread.Sleep(200);

        }
        protected override void ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            MessageService.Success(msg);
            StateHasChanged();
            Thread.Sleep(200);
        }



        #region 生命周期方法增加统一异常处理拦截器
        /*
         * 肉夹馍的aop有基于规则的匹配方式，但有点复杂，
         * 还是决定使用硬编码方式配置，比较稳妥。即 哪里需要就在哪里加 [AbpExceptionInterceptor]
         * 
         * 父类加了，子类再加这个特征的话会重复，会比较浪费。但是父类不加，如果子类没重写并加拦截器，会导致拦截器无法执行。
         * 所以还是决定在抽象中添加，子类可以重写时不调用父类，自己单独加 [AbpExceptionInterceptor]
         * 最坏的情况是子类重写，且必须调用父类方法时，确实比较浪费，层次不深的话也无所谓了。
         */
#if !DEBUG
        [AbpExceptionInterceptor]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        [AbpExceptionInterceptor]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpExceptionInterceptor]
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
#endif
        #endregion
    }
}