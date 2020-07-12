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

namespace BXJG.Utils.File
{
    /// <summary>
    /// 一个临时的文件处理类，配合FileController
    /// </summary>
    public class TempFileManager : DomainService//, ITransientDependency
    {
        //IWebHostEnvironment env
        // private readonly IEnv env;
        string root;
        public TempFileManager(IEnv env)
        {
            //this.env = env;
            root = env.Root;
        }

        public async Task<IEnumerable<Output>> UploadAsync(CancellationToken cancellationToken = default, params Input[] inputs)
        {
            var outputs = new Collection<Output>();
            foreach (var item in inputs)
            {
                //判断文件类型和大小

                var hz = Path.GetExtension(item.FileName);//.jpg
                var wjm = Guid.NewGuid().ToString("n") + hz;//xxx.jpg  xxx=guid
                var wj = Path.Combine(root, "upload", "temp", wjm); ////root/upload/temp/xxx.jpg

                using (var fs = System.IO.File.Create(wj))
                {
                    await item.Stream.CopyToAsync(fs, cancellationToken);
                }
                outputs.Add(new Output(Path.Combine("upload", "temp", wjm)));
            }
            return outputs;
        }

        public void MoveAsync(CancellationToken cancellationToken = default, params string[] inputs)
        {
            foreach (var item in inputs)
            {
                var f = Path.Combine(root, item);

                System.IO.File.Move(f, Path.Combine(root, "upload", Path.GetFileName(item)));
            }
        }
    }
}
