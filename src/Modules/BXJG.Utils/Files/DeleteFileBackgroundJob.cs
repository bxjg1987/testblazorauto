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

    
    public class DeleteFileBackgroundJob : IBackgroundJob<IEnumerable<string>>, ITransientDependency
    {
        public void Execute(IEnumerable<string> args)
        {
            foreach (var item in args)
            {
                File.Delete(item);
            }
        }
    }
}
