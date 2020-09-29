using Abp.DynamicEntityProperties;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class ProductDynamicEntityPropertyDefinition : DynamicEntityPropertyDefinitionProvider
    {
        public override void SetDynamicEntityProperties(IDynamicEntityPropertyDefinitionContext context)
        {
            try
            {
                context.Manager.AddAllowedInputType<SingleLineStringInputType>();
                context.Manager.AddAllowedInputType<CheckboxInputType>();
                context.Manager.AddAllowedInputType<ComboboxInputType>();
            }
            catch { }

            context.Manager.AddEntity<ItemEntity, long>();
        }
    }
}
