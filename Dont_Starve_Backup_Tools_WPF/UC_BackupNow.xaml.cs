using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dont_Starve_Backup_Tools_WPF
{    
    /// <summary>
    /// UC_BackupNow.xaml 的交互逻辑
    /// </summary>
    public partial class UC_BackupNow : UserControl
    {
        string lastBackup = "本次运行暂未进行备份";

        private static UC_BackupNow instance = null;
        private UC_BackupNow()
        {
            InitializeComponent();
            tblock_latest.Text = lastBackup;
        }
        public static UC_BackupNow GetInstance()
        {
            if (instance == null)
                instance = new UC_BackupNow();
            //instance.tblock_latest.Text = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "LatestBackupPath", "暂未备份");
            return instance;
        }

        private void btn_DoBackupNow_Click(object sender, RoutedEventArgs e)
        {
            string backupPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            string filePath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
            if (filePath.Trim().ToUpper() == "未设置" || backupPath.Trim().ToUpper() == "未设置")
            {
                MessageBox.Show("请先设置存档与备份路径！");
            }
            else
            {                
                string latest = DoWork.BackUp(filePath, backupPath);
                tblock_latest.Text = latest.Substring(latest.LastIndexOf(@"\") + 1);
                lastBackup = tblock_latest.Text;
            }
        }
    }
}
