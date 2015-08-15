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
    /// UC_Backup.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Backup : UserControl
    {
        private static UC_Backup instance = null;
        private UC_Backup()
        {
            InitializeComponent();
            string key = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "BackupMode", "BackupNow", "true");
            if (key.ToUpper().Trim() == "TRUE")
            {
                btn_backupNow.SetResourceReference(Button.StyleProperty, "myBtnStyle2");
                btn_backupTiming.SetResourceReference(Button.StyleProperty, "myBtnStyle");
                lb_now.Visibility = Visibility.Visible;
                lb_timing.Visibility = Visibility.Hidden;
                UC_BackupNow ubn = UC_BackupNow.GetInstance();
                bd_ub.Child = ubn;
            }
            else
            {
                btn_backupNow.SetResourceReference(Button.StyleProperty, "myBtnStyle");
                btn_backupTiming.SetResourceReference(Button.StyleProperty, "myBtnStyle2");
                UC_BackupTiming ubt = UC_BackupTiming.GetInstance();
                bd_ub.Child = ubt;
                lb_now.Visibility = Visibility.Hidden;
                lb_timing.Visibility = Visibility.Visible;
            }
        }

        public static UC_Backup GetInstace()
        {
            if (instance == null)
                instance = new UC_Backup();
            return instance;
        }

        private void btn_backupNow_Click(object sender, RoutedEventArgs e)
        {
            btn_backupNow.SetResourceReference(Button.StyleProperty, "myBtnStyle2");
            btn_backupTiming.SetResourceReference(Button.StyleProperty, "myBtnStyle");
            UC_BackupNow ubn = UC_BackupNow.GetInstance();
            bd_ub.Child = ubn;
            lb_now.Visibility = Visibility.Visible;
            lb_timing.Visibility = Visibility.Hidden;
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "BackupMode", "BackupNow", "True");
        }

        private void btn_backupTiming_Click(object sender, RoutedEventArgs e)
        {
            btn_backupNow.SetResourceReference(Button.StyleProperty, "myBtnStyle");
            btn_backupTiming.SetResourceReference(Button.StyleProperty, "myBtnStyle2");
            UC_BackupTiming ubt = UC_BackupTiming.GetInstance();
            bd_ub.Child = ubt;
            lb_now.Visibility = Visibility.Hidden;
            lb_timing.Visibility = Visibility.Visible;
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "BackupMode", "BackupNow", "False");
        }

        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            if (path.Trim().ToUpper() == "未设置")
            {
                MessageBox.Show("尚未设置备份文件夹");
                return;
            }
            else
            {
                System.Diagnostics.Process.Start("explorer.exe", @path);
            }
        }
    }
}
