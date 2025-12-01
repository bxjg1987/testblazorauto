using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Settings
{
    public class SettingEditDto:ISettingValue
    {
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
