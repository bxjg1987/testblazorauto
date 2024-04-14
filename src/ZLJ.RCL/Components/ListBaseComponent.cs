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
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
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
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class ListBaseComponent<TAppService,
                                            TEntityDto,
                                            TPrimaryKey,
                                            TGetAllInput,
                                            TCreateInput,
                                            TUpdateInput> : BXJG.Utils.RCL.Components.ListBaseComponent<TAppService,
                                            TEntityDto,
                                            TPrimaryKey,
                                            TGetAllInput,
                                            TCreateInput,
                                            TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableObj//, new()
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
      
        /// <summary>
        /// 对ant表格的引用
        /// </summary>
        protected Table<TEntityDto> table;

        protected virtual async Task OnQuery(QueryModel condition)
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

            await LoadListData();

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
        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _ = MessageService.Error(msg);
            await Task.Delay(200);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);
            await Task.Delay(200);
        }
    }
}