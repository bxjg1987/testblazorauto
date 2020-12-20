using Abp.DynamicEntityProperties;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.DynamicProperty
{
    public class DynamicPropertyDefinition : DynamicEntityPropertyDefinitionProvider
    {
        public override void SetDynamicEntityProperties(IDynamicEntityPropertyDefinitionContext context)
        {
            //这个是通用的，移动到主程序的ZLJ.Core中去了
            try
            {
                context.Manager.AddAllowedInputType<SingleLineStringInputType>();
                context.Manager.AddAllowedInputType<CheckboxInputType>();
                context.Manager.AddAllowedInputType<ComboboxInputType>();
            }
            catch { }
            context.Manager.AddEntity<SkuEntity, long>();
        }
    }
}
