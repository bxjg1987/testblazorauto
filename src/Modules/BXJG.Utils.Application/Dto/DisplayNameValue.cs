using Abp;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Dto
{
    public class DisplayNameValue<T> : NameValueDto<T>
    {
        public DisplayNameValue()
        {
        }

        public DisplayNameValue(NameValue<T> nameValue) : base(nameValue)
        {
        }

        public DisplayNameValue(string name, T value) : base(name, value)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is DisplayNameValue<T> value &&
                   Name == value.Name &&
                   Value.ToString() == value.Value.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
