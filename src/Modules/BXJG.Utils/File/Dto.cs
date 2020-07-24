using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.File
{
    public class Input
    {
        public Input(string fileName, Stream stream, string contentType,long lenght)
        {
            FileName = fileName;
            Stream = stream;
            ContentType = contentType;
            Length = lenght;
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }
        public string ContentType { get; private set; }
        public long Length { get; private set; }
    }

    public class Output
    {
        public Output(string relativePath)
        {
            RelativePath = relativePath;
        }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }
    }
}
