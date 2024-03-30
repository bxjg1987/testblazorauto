using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using BXJG.Common;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.Application.File
{
    /// <summary>
    /// 后台定时任务作为用例入口，应该定义在应用层
    /// 具体的应用模块中注册此定时任务
    /// </summary>
    public class RemoveUploadFileWorker : PeriodicBackgroundWorkerBase, ITransientDependency
    {
        ///// <summary>
        ///// 获取安全的上传目录，一般是非应用程序目录 可读写 不可执行
        ///// </summary>
        //protected string _uploadDir;
        /// <summary>
        /// 安全上传时的临时目录
        /// </summary>
        protected string _tempDir;

        public RemoveUploadFileWorker(AbpTimer timer, IConfiguration Configuration) : base(timer)
        {
            Timer.Period = 1000 * 60 * 10;//时间别太长，默认情况下应用长时间不被访问，应用会终止
                                          //未考虑并发冲突，也基本不需要
            var _uploadDir = Configuration["Upload:SaveDir"]; // d:\app\wwwroot\upload 
            _tempDir = Path.Combine(_uploadDir, BXJGUtilsConsts.UploadTemp); // d:\app\wwwroot\upload\temp
        }
        //[UnitOfWork]
        protected override void DoWork()
        {
            if (!Directory.Exists(_tempDir))
                return;

            var files = Directory.GetFiles(_tempDir);
            foreach (var item in files)
            {
                var file = new FileInfo(item);
                if ((Clock.Now - file.CreationTime).TotalMinutes > 30) //这个分钟数可以搞到settings系统中
                {
                    try
                    {
                        System.IO.File.Delete(item);
                    }
                    catch { }
                }
            }
            //var dirs = Directory.GetDirectories(dir);
            //if (dirs.Length > 0)
            //{
            //    foreach (var item in dirs)
            //    {
            //        DoWork(item);
            //    }
            //}
        }
    }
}
