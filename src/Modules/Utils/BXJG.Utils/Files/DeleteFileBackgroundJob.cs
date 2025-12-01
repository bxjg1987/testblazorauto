using Abp.BackgroundJobs;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{
    //不用批量删，因为这里是希望尽可能删掉，异常时后台任务会重试，若尝试批量删，前面删除的异常会导致后面的文件无法尝试删除
    public class DeleteFileBackgroundJob : IBackgroundJob<string>, ITransientDependency
    {
        public void Execute(string args)
        {
            File.Delete(args);
        }
    }
}