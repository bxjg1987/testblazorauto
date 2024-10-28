using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    public interface IHaveIoc
    {
        IIocResolver IocResolver { get; set; }
    }
}
