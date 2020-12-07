using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Common
{
    public interface IEnv
    {
        string WebRoot { get; }
        string RootUrl { get; }
    }

    public class DefaultWebEnv : IEnv
    {
        public string WebRoot
        {
            get
            {
                return Path.Combine(AppContext.BaseDirectory, "wwwroot");
            }
        }
        public string RootUrl => 
    }
}
