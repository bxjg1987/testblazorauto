using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.Share.Files
{
    [Obsolete("看样子好像是没用了，如果后续发现有用就去掉这个标签")]
    public class FileInput
    {
        public FileInput(string fileName, Stream stream, string ct)
        {
            FileName = fileName;
            Stream = stream;
            ContentType = ct;
        }
        public string ContentType { get; set; }
        public string FileName { get; private set; }
        public Stream Stream { get; private set; }
        public long Length => Stream.Length;
    }

    //public class MoveResult
    //{
    //    public MoveResult(string relativePath, string thum = default)
    //    {
    //        RelativePath = relativePath;
    //        Thum = thum;
    //    }

    //    /// <summary>
    //    /// 相对路径
    //    /// </summary>
    //    public string RelativePath { get; set; }
    //    /// <summary>
    //    /// 缩略图（目前只考虑图像类型的文件生成缩略图，将也许增加其它类型文件生成缩略图，如：视频、pdf等）
    //    /// </summary>
    //    public string Thum { get; set; }
    //}
}
