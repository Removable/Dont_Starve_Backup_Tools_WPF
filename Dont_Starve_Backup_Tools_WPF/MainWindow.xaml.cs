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
using MahApps.Metro.Controls;

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static MainWindow instance;
        public MainWindow()
        {
            InitializeComponent();
            btn_back.Visibility = Visibility.Hidden;
            if (Environment.CurrentDirectory.Substring(Environment.CurrentDirectory.LastIndexOf(@"\") + 1) == "resource")
            {
                MessageBox.Show("请勿从该位置启动");
                this.Close();
            }
        }
        public static MainWindow GetInstance()
        {
            if (instance == null)
                instance = new MainWindow();
            return instance;
        }

        //返回按钮
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            UC_BackupTiming ucb = UC_BackupTiming.GetInstance();
            if (ucb.btn_stopTiming.IsEnabled == true)
            {
                return;
            }
            bd_main.Child = gd_main;
            btn_back.Visibility = Visibility.Hidden;
        }

        #region 备份按钮
        private void btn_GuideBackup_MouseMove(object sender, MouseEventArgs e)
        {
            btn_GuideBackup.SetResourceReference(Button.ContentProperty, "btn-backupico");
        }
        private void btn_GuideBackup_Click(object sender, RoutedEventArgs e)
        {
            btn_back.Visibility = Visibility.Visible;
            UC_Backup ucb = UC_Backup.GetInstace();
            bd_main.Child = ucb;
        }
        private void btn_GuideBackup_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_GuideBackup.SetResourceReference(Button.ContentProperty, "btn-backupword");
        }
        #endregion

        #region 还原按钮
        private void btn_GuideStore_MouseMove(object sender, MouseEventArgs e)
        {
            btn_GuideStore.SetResourceReference(Button.ContentProperty, "btn-restoreico");
        }
        private void btn_GuideStore_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_GuideStore.SetResourceReference(Button.ContentProperty, "btn-restoreword");
        }
        private void btn_GuideStore_Click(object sender, RoutedEventArgs e)
        {
            UC_Restore ucr = UC_Restore.GetInstance();
            bd_main.Child = ucr;
            ucr.AddItemToListBox();
            btn_back.Visibility = Visibility.Visible;
        }
        #endregion

        #region 设置按钮
        private void btn_GuideSettings_MouseMove(object sender, MouseEventArgs e)
        {
            btn_GuideSettings.SetResourceReference(Button.ContentProperty, "btn-settingsico");
        }

        private void btn_GuideSettings_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_GuideSettings.SetResourceReference(Button.ContentProperty, "btn-settingsword");
        }
        private void btn_GuideSettings_Click(object sender, RoutedEventArgs e)
        {
            UC_Settings ucs = UC_Settings.GetInstance();
            bd_main.Child = ucs;
            btn_back.Visibility = Visibility.Visible;
        }
        #endregion

        #region 关于按钮
        private void btn_GuideAbout_Click(object sender, RoutedEventArgs e)
        {
            btn_back.Visibility = Visibility.Visible;
            UC_About uca = UC_About.GetInstance();
            bd_main.Child = uca;
        }
        private void btn_GuideAbout_MouseMove(object sender, MouseEventArgs e)
        {
            btn_GuideAbout.SetResourceReference(Button.ContentProperty, "btn-aboutico");
        }
        private void btn_GuideAbout_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_GuideAbout.SetResourceReference(Button.ContentProperty, "btn-aboutword");
        }
        #endregion

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UC_BackupTiming ucb = UC_BackupTiming.GetInstance();
            if (ucb.btn_stopTiming.IsEnabled == true)
            {
                System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("正在进行定时备份，确认退出？", "警告", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            instance = this;
        }
        //public static void SetOwner(MainWindow mw)
        //{
        //    UpdateWindow uw = UpdateWindow.GetInstance();
        //    uw.Owner = mw;
        //}
    }
}
