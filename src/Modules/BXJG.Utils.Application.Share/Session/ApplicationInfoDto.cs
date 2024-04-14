using System;
using System.Collections.Generic;

namespace BXJG.Utils.Application.Share.Session
{
    public class ApplicationInfoDto
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Dictionary<string, bool> Features { get; set; }

        public string RunTimeVersion { get; set; }

        public string AbpVersion { get; set; }

    }
}
