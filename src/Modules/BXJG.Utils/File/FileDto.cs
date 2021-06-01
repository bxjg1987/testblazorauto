using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.File
{
    public class FileDto
    {
        [Obsolete]
        public string FilePath { get; set; }
        [Obsolete]
        public string ThumPath { get; set; }

        public string FileUrl { get; set; }
        public string ThumUrl { get; set; }
    }
}
