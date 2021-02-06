using Abp.DynamicEntityProperties;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ
{
    public class ZLJDynamicEntityPropertyDefinition : DynamicEntityPropertyDefinitionProvider
    {
        public override void SetDynamicEntityProperties(IDynamicEntityPropertyDefinitionContext context)
        {
            context.Manager.AddAllowedInputType<SingleLineStringInputType>();
            context.Manager.AddAllowedInputType<CheckboxInputType>();
            context.Manager.AddAllowedInputType<ComboboxInputType>();
        }
    }
}
