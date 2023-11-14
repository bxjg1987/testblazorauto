using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBlazor.Components
{
    /*
     * ModalService.CreateModalAsync要求弹窗内容组件继承FeedbackComponent<TComponentOptions>
     * 但我们的新增或详情表单有自己的父类，不方便
     */
    public class FileName<TComponent, TComponentOptions> : FeedbackComponent<TComponentOptions>
    {

    }
}
