using BXJG.Utils.Share.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share.Files
{
    /// <summary>
    /// 临时上传文件的返回数据模型
    /// </summary>
    [Obsolete("请直接使用BXJG.Utils.Share.Files.SetAttachmentFile")]
    public class UploadTempFileDto: SetAttachmentFile
    {
        [Obsolete("请直接使用BXJG.Utils.Share.Files.SetAttachmentFile.TempPath")]
        public string Path { get=>base.TempPath; set=>base.TempPath=value; }
        [Obsolete("请直接使用BXJG.Utils.Share.Files.SetAttachmentFile.FileName")]
        public string Name { get => base.FileName; set => base.FileName = value; }
    }
}
