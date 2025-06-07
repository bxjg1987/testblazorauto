using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Extensions;
using AntDesign;
using AntDesign.TableModels;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Application.Share;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Html;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
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
     * 
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 虽然当前项目直接依赖antblazor，且此当前抽象列表组件也依赖它
     * 但我们实现逻辑是尽量考虑标准的抽象列表组件，因此在组件中直接包含PageSize、PageIndex等属性，
     * 而不是完全依赖antblazor的套路
     * 这样，将来我们需要抽象一个标准的列表组件时，这里的大部分代码是可以复制到抽象中的。
     * 
     * 它仅仅定义列表相关功能，并不包含新增、修改等弹窗相关内容，那个交给子类去实现，因为有列表不一定需要弹窗
     */

    /// <summary>
    /// 抽象的，基于ant table的列表页抽象组件
    /// </summary>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public abstract class ListBaseComponent<TEntityDto,
                                            TPrimaryKey,
                                            TGetAllInput> : BXJG.Utils.RCL.Components.ListBaseComponent<TEntityDto,
                                                                                                        TPrimaryKey,
                                                                                                        TGetAllInput>
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableObj//, new()
        where TGetAllInput : new()
    {
        [Inject]
        public IWebAssemblyHostEnvironment Environment { get; set; }

        //它不是用例，但因为列表是异步的，这里是核心，所以就把异常加这里吧
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task LoadCore()
        {
            await base.LoadCore();
        }

        //各按钮的异常处理还是加上

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
        protected override void BtnClearFilterClick()
        {
            base.BtnClearFilterClick();
        }

#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override void BtnSearchClick()
        {
            base.BtnSearchClick();
        }

        //删除也是异步的，加上异常



#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnDeleteClick()
        {
          await  base.BtnDeleteClick();
        }



#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override  async Task BtnDeleteItemClick(TEntityDto item)
        {
          await  base.BtnDeleteItemClick(item);
        }

        /// <summary>
        /// 对ant表格的引用
        /// </summary>
        protected Table<TEntityDto> table;

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
            this.PageSize = condition.PageSize;
            this.PageIndex = condition.PageIndex;
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

        /*
         * MessageService内部估计会异步刷新ui
         * await它的话，要显示显示隐藏后才结束
         * 同步方法中，多次调用StateChange貌似没有
         */

        public override string Sorting { get => base.Sorting;
            set 
            {
                //var lspx = value.Replace(" Descing", " desc").Replace(" Ascing", " asc").Replace(" None", "");
                var strSort = "";
                if (value.IsNotNullOrWhiteSpaceBXJG())
                {
                    var sddsf = value.Split(',');
                    foreach (var item in sddsf)
                    {
                        if (item.EndsWith(" Descing"))
                            strSort += item.Replace(" Descing", " desc")+",";
                        else if (item.EndsWith(" Ascing"))
                            strSort += item.Replace(" Ascing", " asc") + ",";
                        
                    }
                }

                    strSort = strSort.TrimEnd(',');
                base.Sorting = strSort;
            } 
        }
        protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            MessageService.Error(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);//碰到这个，开始刷新
        }
        protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            MessageService.Success(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);//碰到这个，开始刷新

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
        #endregion
    }
}