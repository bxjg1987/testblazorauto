using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /*
     * 套餐算法不太，所需要的属性不同，不同算法可能使用相同的属性，因此属性与算法是多对多关系
     * 但算法基本就那几种，变动性不是特别强。可以将所有算法需要使用到的属性规定死，若将来增加
     * 新的算法确实需要使用到新属性时再重构。
     * 这种环境下不同算法所关联的属性并不需要完全动态，所以称为半动态
     * 
     */

    /// <summary>
    /// 半动态属性
    /// </summary>
    public class SemiDynamicProperty
    {
        public SemiDynamicProperty(string propertyName, string propertyDisplayName, ICollection<ValidationAttribute> validators)
        {
            PropertyName = propertyName;
            PropertyDisplayName = propertyDisplayName;
            Validators = validators.ToList();
        }

        public string PropertyName { get; init; }
        public string PropertyDisplayName { get; init; }
        public IReadOnlyCollection<ValidationAttribute> Validators { get; init; }
    }

    //public class SemiDynamicPropertyDto {
    //    public string PropertyName { get; set; }
    //    public string PropertyDisplayName { get; set; }
    //    public IReadOnlyCollection<ValidationAttribute> Validators { get; set; }
    //}
    //public class ValidationDto { 

    //}
}
