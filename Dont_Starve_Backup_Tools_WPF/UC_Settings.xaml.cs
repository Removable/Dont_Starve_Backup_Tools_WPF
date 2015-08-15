using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// UC_Settings.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Settings : System.Windows.Controls.UserControl
    {
        private static UC_Settings instance = null;
        private UC_Settings()
        {
            InitializeComponent();
        }
        public static UC_Settings GetInstance()
        {
            if (instance == null)
                instance = new UC_Settings();
            instance.tb_filePath.Text = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
            instance.tb_backupPath.Text = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            string temp = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Settings", "GameType", "Null");
            switch (temp.ToUpper())
            {
                case "GENUINE":
                    instance.lb_db.Visibility = Visibility.Hidden;
                    instance.lb_zb.Visibility = Visibility.Visible;
                    break;
                case "PIRATE":
                    instance.lb_zb.Visibility = Visibility.Hidden;
                    instance.lb_db.Visibility = Visibility.Visible;
                    break;
                case "NULL":
                    instance.lb_zb.Visibility = Visibility.Hidden;
                    instance.lb_db.Visibility = Visibility.Hidden;
                    break;
            }
            return instance;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Settings", "GameType", "Null") == "Null")
            {
                System.Windows.MessageBox.Show("请选择您的游戏类型");
                return;
            }
            if (MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Settings", "GameType", "Null") == "Genuine")
            {
                try
                {
                    FileInfo fi = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Klei\DoNotStarve\settings.ini");
                    if(!fi.Exists)
                    {
                        System.Windows.MessageBox.Show("您似乎没有安装steam正版饥荒游戏哦~");
                        return;
                    }
                    string temp = MyProfiles.INIGetStringValue(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Klei\DoNotStarve\settings.ini", "STEAM", "DISABLECLOUD", "Null");
                    switch (temp.ToUpper())
                    {
                        case "NULL":
                            System.Windows.MessageBox.Show("您似乎没有安装steam正版饥荒游戏哦~");
                            return;
                        case "FALSE":
                            string steamPath;
                            RegistryKey current = Registry.CurrentUser;
                            RegistryKey Software = current.OpenSubKey("SOFTWARE", true);
                            RegistryKey Valve = Software.OpenSubKey("Valve", true);
                            RegistryKey Steam = Valve.OpenSubKey("Steam", true);
                            steamPath = Steam.GetValue("SteamPath").ToString();
                            DirectoryInfo di2 = new DirectoryInfo(@steamPath + @"\userdata");
                            DoWork.Scanner(di2, tb_filePath);
                            if (tb_filePath.Text == "未设置")
                            {
                                System.Windows.MessageBox.Show("啊哦，没有找到存档~");
                            }
                            break;
                        case "TRUE":
                            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Klei\DoNotStarve\save");
                            DoWork.Scanner(di, tb_filePath);
                            if (tb_filePath.Text == "未设置")
                            {
                                System.Windows.MessageBox.Show("啊哦，没有找到存档~");
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("啊哦，搜索出错了，请检查是否正确的选择了游戏版本~");
                }


            }
            if (MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Settings", "GameType", "Null") == "Pirate")
            {
                DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Klei\DoNotStarve\save");
                DoWork.Scanner(di, tb_filePath);
                if (tb_filePath.Text == "未设置")
                {
                    DirectoryInfo di2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\SKIDROW\219740\Storage");
                    DoWork.Scanner(di2, tb_filePath);
                    if (tb_filePath.Text == "未设置")
                    {
                        System.Windows.MessageBox.Show("啊哦，没有找到存档~");
                    }
                }
            }
        }

        private void btn_zb_Click(object sender, RoutedEventArgs e)
        {
            if (lb_zb.Visibility != Visibility.Visible)
                tb_filePath.Text = "未设置";
            lb_zb.Visibility = Visibility.Visible;
            lb_db.Visibility = Visibility.Hidden;
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "Settings", "GameType", "Genuine");
        }

        private void btn_db_Click(object sender, RoutedEventArgs e)
        {
            if (lb_db.Visibility != Visibility.Visible)
                tb_filePath.Text = "未设置";
            lb_zb.Visibility = Visibility.Hidden;
            lb_db.Visibility = Visibility.Visible;
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "Settings", "GameType", "Pirate");
        }

        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            if (tb_filePath.Text == "未设置")
                return;
            System.Diagnostics.Process.Start("explorer.exe", @tb_filePath.Text);
        }

        private void btn_settingFPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tb_filePath.Text = fbd.SelectedPath;
                }
            }
            catch (Exception pathErr)
            {

                System.Windows.MessageBox.Show("错误：" + pathErr.Message);
            }
        }

        private void btn_settingBPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tb_backupPath.Text = fbd.SelectedPath;
                }
            }
            catch (Exception pathErr)
            {

                System.Windows.MessageBox.Show("错误：" + pathErr.Message);
            }
        }

        private void btn_saveChange_Click(object sender, RoutedEventArgs e)
        {
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "Path", "FilePath", tb_filePath.Text);
            MyProfiles.INIWriteValue(StaticResource.iniFilePath, "Path", "BackupPath", tb_backupPath.Text);
            System.Windows.MessageBox.Show("保存成功！");
        }

    }
}
