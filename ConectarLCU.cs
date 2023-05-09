using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLoL_v1._0_
{
    internal class ConectarLCU
    {
        private Regex cmdLine = new Regex("/c wmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline");
        private string pass, port;
        private Regex pass_regex = new Regex("--remoting-auth-token=([\\w-]*)");
        private Regex port_regex = new Regex("--app-port=([0-9]*)");

        public string LCUPassPort()
        {
            Process cmd = new Process();
            cmd.StartInfo = new ProcessStartInfo("cmd")
            {
                UseShellExecute= false,
                RedirectStandardOutput = true,
                CreateNoWindow= true,
                Arguments = cmdLine.ToString()
            };
            cmd.Start();
            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            pass = pass_regex.Match(output).Groups[1].Value;
            port = port_regex.Match(output).Groups[1].Value;
            return $"{pass}:{port}";
        }

        public bool IsLCUOpen()
        {            
            var lcu = Process.GetProcessesByName("LeagueClientUx");
            if (lcu.Length > 0) return true;
            return false;
        }

    }
}
