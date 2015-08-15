using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// UC_BackupTiming.xaml 的交互逻辑
    /// </summary>
    public partial class UC_BackupTiming : UserControl
    {
        DispatcherTimer cTimer = new DispatcherTimer();
        private static UC_BackupTiming instance = null;
        string backupPath = null;
        string filePath = null;
        private UC_BackupTiming()
        {
            InitializeComponent();
            btn_stopTiming.IsEnabled = false;
            cTimer.Interval = TimeSpan.FromSeconds(1);
            cTimer.Tick += cTimer_Tick;
        }

        void cTimer_Tick(object sender, EventArgs e)
        {
            DoWork.CountDown(this.tblock_min, this.tblock_sec, int.Parse(tb_min.Text), MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置"), MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置"));
        }

        void bcTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DoWork.BackUp(filePath, backupPath);
        }
        public static UC_BackupTiming GetInstance()
        {
            if (instance == null)
                instance = new UC_BackupTiming();
            return instance;
        }

        private void btn_startTiming_Click(object sender, RoutedEventArgs e)
        {
            backupPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            filePath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
            if (backupPath.ToUpper() == "未设置" || filePath.ToUpper() == "未设置")
            {
                MessageBox.Show("请先设置存档与备份路径！");
                return;
            }
            tblock_min.Text = tb_min.Text.PadLeft(2, '0');
            tblock_sec.Text = "00";
            cTimer.Start();
            btn_startTiming.IsEnabled = false;
            btn_stopTiming.IsEnabled = true;
            s_min.IsEnabled = false;
            UC_Backup ucb = UC_Backup.GetInstace();
            ucb.btn_backupNow.IsEnabled = false;
            ucb.btn_backupTiming.IsEnabled = false;
        }

        private void btn_stopTiming_Click(object sender, RoutedEventArgs e)
        {
            cTimer.Stop();
            tblock_sec.Text = "00";
            tblock_min.Text = "00";
            btn_startTiming.IsEnabled = true;
            btn_stopTiming.IsEnabled = false;
            s_min.IsEnabled = true;
            UC_Backup ucb = UC_Backup.GetInstace();
            ucb.btn_backupNow.IsEnabled = true;
            ucb.btn_backupTiming.IsEnabled = true;
        }
    }
}
