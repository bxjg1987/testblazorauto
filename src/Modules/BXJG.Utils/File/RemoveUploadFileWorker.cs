using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using BXJG.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.File
{
    public class RemoveUploadFileWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        string dir;
        public RemoveUploadFileWorker(AbpTimer timer, IEnv env) : base(timer)
        {
            dir = Path.Combine(env.Root,Consts.UploadTemp);
            Timer.Period = 1000 * 60;//时间别太长，默认情况下应用长时间不被访问，应用会终止
        }
        //[UnitOfWork]
        protected override void DoWork()
        {
            var files = Directory.GetFiles(dir);
            foreach (var item in files)
            {
                var file = new FileInfo(item);
                if ((DateTime.Now - file.CreationTime).TotalMinutes > 30)
                    System.IO.File.Delete(item);
            }
        }
    }
}
