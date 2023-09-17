using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Components
{
    /// <summary>
    /// 用来承载新增组件的组件，可以看作是个容器。（查看详情、修改 是用单独的承载组件）
    /// 弹窗、tab、页面可以继承它，它内部应该承载一个具体的新增组件
    /// </summary>
    public class AbpMudCarryingCreateBaseComponent : AbpMudBaseComponent
    {
        ///// <summary>
        ///// 保存后是否继续新增
        ///// </summary>
        //protected bool SaveAndContinue = false;
        ///// <summary>
        ///// 新增组件类型
        ///// </summary>
        //[Parameter]
        //public Type? CreateComponentType { get; set; }
        ///// <summary>
        ///// 新增组件
        ///// </summary>
        //protected DynamicComponent? CreateComponent;

        public IFormCreate Form { get; set; }
        /// <summary>
        /// 是否正在保存
        /// </summary>
        protected bool Saving = false;
        /// <summary>
        /// 点击保存按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SaveClick()
        {
            Saving = true;
            //安全执行不要放新增组件内部，这样可以简化新增组件的开发
            //不要考虑新增组件可能在某处单独使用，那种情况也在外面包一层承载组件
            await base.SafelyExecuteAsync(async () =>
            {
                await Form.Save();
                //var frm = CreateComponent!.Instance as IForm;
                //await frm!.Save();
                //if (SaveAndContinue)
                //    await frm!.Reset();
            });
            Saving = false;
        }
    }
    /// <summary>
    /// 具体的新增组件应该实现此接口
    /// </summary>
    public interface IForm
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        Task Save();
    }
    /// <summary>
    /// 新增表单组件需要实现此接口
    /// </summary>
    public interface IFormCreate : IForm
    {
        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        ValueTask Reset();
    }
}