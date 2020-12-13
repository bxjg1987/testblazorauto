using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.MiniProgram
{
    public interface ILoginHandler
    {
        Task LoginAsync(LoginContext context);
    }
}
