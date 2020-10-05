using BXJG.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ
{
    public class NetCoreEnv : IEnv
    {
        IWebHostEnvironment webHostEnvironment;
        public NetCoreEnv(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string Root => webHostEnvironment.WebRootPath;
    }
}
