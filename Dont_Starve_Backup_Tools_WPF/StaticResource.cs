using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dont_Starve_Backup_Tools_WPF
{
    public static class StaticResource
    {
        public static string iniFilePath = Environment.CurrentDirectory + @"\resource\config.ini";
        public static string updatelogFilePath = Environment.CurrentDirectory + @"\resource\updatelog";
        public static string[] filesName = new string[] { "boot_modindex", "modindex", "saveindex", " survival_1", "survival_2", "survival_3", "survival_4", "survival_5" };
        public static string DownloadUri = "";
    }
}
