using Abp.Dependency;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using BXJG.Common;
using Abp.Configuration;
using Abp.Threading.Extensions;
using System.Linq;
using Abp.UI;

namespace BXJG.Utils.File
{
    public class TempFileManager : DomainService//, ITransientDependency
    {
        private readonly ISettingManager settingManager;
        string dir;
        string tempDir;
        public TempFileManager(IEnv env, ISettingManager settingManager)
        {
            this.dir = Path.Combine(env.Root, Consts.UploadDir);
            this.tempDir = Path.Combine(env.Root, Consts.UploadTemp);
            this.settingManager = settingManager;
        }

        public async Task<IList<Output>> UploadAsync(CancellationToken cancellationToken = default, params Input[] inputs)
        {
            var outputs = new List<Output>();
            var ts = await settingManager.GetSettingValueAsync(Consts.SettingKeyUploadType);
            var aryts = ts.Split(',');

            var sz = await settingManager.GetSettingValueAsync<int>(Consts.SettingKeyUploadSize);

            foreach (var item in inputs)
            {
                //这里的类型后期按mime做个对应
                if (!aryts.Contains(item.ContentType, StringComparer.OrdinalIgnoreCase))
                    throw new UserFriendlyException($"不允许上传此类型的文件，仅允许{ts}");

                if (item.Length > sz * 1024)
                    throw new UserFriendlyException($"上传的文件大小超过限制，最大为{sz}Kb");

                var hz = Path.GetExtension(item.FileName);//.jpg
                var wjm = Guid.NewGuid().ToString("n") + hz;//xxx.jpg  xxx=guid
                var wj = Path.Combine(tempDir, wjm); ////root/upload/temp/xxx.jpg

                using (var fs = System.IO.File.Create(wj))
                {
                    await item.Stream.CopyToAsync(fs, cancellationToken);
                }
                outputs.Add(new Output(Path.Combine(Consts.UploadTemp, wjm)));
            }
            return outputs;
        }

        public void Move( params string[] inputs)
        {
            foreach (var item in inputs)
            {
                var fileName = Path.GetFileName(item);
                var f = Path.Combine(this.tempDir, fileName);
                System.IO.File.Move(f, Path.Combine(this.dir, fileName));
            }
        }
    }
}
