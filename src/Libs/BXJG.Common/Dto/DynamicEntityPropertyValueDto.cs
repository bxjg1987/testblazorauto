using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    public class DynamicEntityPropertyValueDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int DynamicEntityPropertyId { get; set; }
        public int DynamicEntityPropertyDynamicPropertyId { get; set; }
        public string DynamicEntityPropertyDynamicPropertyPropertyName { get; set; }
    }
}
