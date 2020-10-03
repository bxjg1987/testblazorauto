using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.File
{
    public class Input
    {
        public Input(string fileName, Stream stream, string contentType)
        {
            FileName = fileName;
            Stream = stream;
            ContentType = contentType;
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }
        public string ContentType { get; private set; }
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
