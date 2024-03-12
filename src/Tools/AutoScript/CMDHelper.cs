using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript
{
    public class CMDHelper
    {

        public static void Execute(string cmd, DataReceivedEventHandler handler, int timeout=0)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c " + cmd,
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Verb = "RunAs"
                //StandardInputEncoding = System.Text.Encoding.UTF8,
                //StandardOutputEncoding = System.Text.Encoding.UTF8,
                //StandardErrorEncoding = System.Text.Encoding.UTF8,
            };

            using var p = new Process();
            
            p.StartInfo = startInfo;
            p.OutputDataReceived += handler;
            p.ErrorDataReceived += handler;
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            if (timeout == 0)
            {
                p.WaitForExit();
            }
            else
            {
                p.WaitForExit(timeout);
            }
            //p.OutputDataReceived -= handler;
            //p.ErrorDataReceived -= handler;
        }
    }
}
