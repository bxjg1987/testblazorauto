using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public class BXJGDynamicProperty //: DynamicObject
    {
        public BXJGDynamicProperty(string name,
                                   string displayName,
                                   string inputType = "text",
                                   string dateTimeFormatter = "yyyy-MM-dd HH:mm:ss",
                                   int decimalPlaces = 2,
                                   IDictionary<string, object> values = default,
                                   ICollection<ValidationAttribute> validators = default,
                                   IDictionary<string, object> additionalData = default)
        {
            Name = name;
            DisplayName = displayName;
            InputType = inputType;
            DateTimeFormatter = dateTimeFormatter;
            Precision = decimalPlaces;
            Values = new ReadOnlyDictionary<string, object>(values ?? new Dictionary<string, object>());
            Validators = validators?.ToList() ?? new List<ValidationAttribute>();
            AdditionalData =new ReadOnlyDictionary<string, object>(additionalData ?? new Dictionary<string, object>());
        }

        public string Name { get; init; }

        public string DisplayName { get; init; }

        public string InputType { get; init; }

        public string DateTimeFormatter { get; init; }

        public int Precision { get; init; }

        public IReadOnlyDictionary<string, object> Values { get; init; }

        public bool MultipleSelect { get; init; }

        public IReadOnlyCollection<ValidationAttribute> Validators { get; init; }

        public int OrderIndex { get; set; }

        public IReadOnlyDictionary<string, object> AdditionalData{ get; set; }
    }

    //public class SemiDynamicPropertyDto {
    //    public string PropertyName { get; set; }
    //    public string PropertyDisplayName { get; set; }
    //    public IReadOnlyCollection<ValidationAttribute> Validators { get; set; }
    //}
    //public class ValidationDto { }
}
