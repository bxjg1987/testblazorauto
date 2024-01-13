using Abp.Application.Features;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Feature
{
    public class ZLJFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            //var sampleBooleanFeature = context.Create("SampleBooleanFeature", defaultValue: "false");
            //sampleBooleanFeature.CreateChildFeature("SampleNumericFeature", defaultValue: "10");
            //context.Create("SampleSelectionFeature", defaultValue: "B");

            //父特征通常定义为布尔特征。仅当启用了父功能时，子功能才可用。 ASP.NET 样板不强制执行此操作，但我们推荐这样做。 应用程序应该处理它。
            var enableCodeGenarator = context.Create("EnableCodeGenarator", defaultValue: "true","是否开启代码生成器".LICommon(),inputType:new CheckboxInputType());
        }
    }
}
