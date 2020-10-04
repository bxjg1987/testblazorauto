using BXJG.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.File
{
    public class DefaultEnv : IEnv
    {
        public string Root
        {
            get
            {
                return Path.Combine(AppContext.BaseDirectory, "wwwroot");
            }
        }
    }
}
