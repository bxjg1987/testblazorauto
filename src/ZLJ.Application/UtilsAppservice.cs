using BXJG.Common.Extensions;
using hyjiacan.py4n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ
{
    public class UtilsAppService : ZLJAppServiceBase, IUtilsAppService
    {
        public string GetPinYinFirstLetter(string chinese, bool toUpper = true)
        {
            return chinese.GetPinYinFirstLetter(toUpper);
        }
    }
}
