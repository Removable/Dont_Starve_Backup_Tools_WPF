using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;
using System.Diagnostics;

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// UC_Restore.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Restore : System.Windows.Controls.UserControl
    {
        private static UC_Restore instance;
        private UC_Restore()
        {
            InitializeComponent();
            AddItemToListBox();
        }
        public static UC_Restore GetInstance()
        {
            if (instance == null)
                instance = new UC_Restore();
            return instance;
        }

        //ListBox添加Item的方法
        public void AddItemToListBox()
        {
            lbox_fileList.Items.Clear();
            string tempPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            if (tempPath == "未设置")
                return;
            string[] direc = Directory.GetDirectories(tempPath);
            foreach (string item in direc)
            {
                int startIndex = item.LastIndexOf(@"\");
                string temp = item.Substring(startIndex + 1, item.Length - startIndex - 1);
                if (temp.Length == 19)
                {
                    string str = item.Substring(item.Length - 20, 2);
                    if (str == @"\" + "2")
                    {
                        this.lbox_fileList.Items.Add(temp);
                    }
                }
            }
        }

        //还原按钮
        private void btn_restore_Click(object sender, RoutedEventArgs e)
        {
            Process[] process = Process.GetProcessesByName("dontstarve_steam");
            if (process != null && process.Length != 0)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("检测到饥荒正在运行，在进行本次还原之前，请退出至主界面，否则可能导致游戏崩溃！请在完成准备之后点击“是”以继续。", "警告", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    System.Windows.MessageBox.Show("还原失败！");
                    return;
                }
            }
            if (lbox_fileList.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("请选择一个存档");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\" + lbox_fileList.SelectedValue.ToString());
            if (!di.Exists)
            {
                AddItemToListBox();
                System.Windows.MessageBox.Show("备份文件不存在！");
                return;
            }
            DoWork.CompulsoryBackup();
            string name = lbox_fileList.SelectedItem.ToString();
            string tempPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
            string[] files = Directory.GetFiles(tempPath);
            foreach (var item in files)
            {
                File.Delete(item);
            }
            DoWork.Restore(name);
            System.Windows.MessageBox.Show("还原成功！");
        }

        //删除按钮
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (lbox_fileList.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("请选择一个存档！");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\" + lbox_fileList.SelectedValue.ToString());
            if (!di.Exists)
            {
                AddItemToListBox();
                System.Windows.MessageBox.Show("备份文件不存在！");
                return;
            }
            string temp = lbox_fileList.SelectedItem.ToString();
            DoWork.DeletBackUp(temp);
            lbox_fileList.Items.Remove(temp);
        }

        //备份位置按钮
        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            string temp = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
            if (temp == "未设置")
                return;
            System.Diagnostics.Process.Start("explorer.exe", @temp);
        }

        //存档位置按钮
        private void btn_openfileFolder_Click(object sender, RoutedEventArgs e)
        {
            string temp = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
            if (temp.ToUpper() == "未设置")
                return;
            System.Diagnostics.Process.Start("explorer.exe", @temp);
        }

        //旧档恢复
        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            DoWork.OldFileRestore();
        }

        //一键转移备份
        private void btn_cutAll_Click(object sender, RoutedEventArgs e)
        {
            if (MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") == "未设置" || MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置") == "未设置")
            {
                System.Windows.MessageBox.Show("请先正确设置好存档与备份的路径！");
                return;
            }
            string newpath = null;
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    newpath = fbd.SelectedPath;
                if (newpath == null)
                    return;
                DoWork.CopyAllBackup(newpath);
                DialogResult dr = System.Windows.Forms.MessageBox.Show("是否将程序设置中的备份位置修改为刚刚选择的文件夹？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    MyProfiles.INIWriteValue(StaticResource.iniFilePath, "Path", "BackupPath", newpath);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        #region 强制备份的提示信息
        private void label_info_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            label_info.Margin = new Thickness(0, 227, 0, 0);
            label_info.Height = 49;
            label_info.Width = 499;
            label_info.Content = "小提示：在每次还原时，小工具都会将现有的存档进行备份，并单独放在一个文件夹内，在\r\n还原的存档出错时，可以点击“旧档恢复”按钮进行恢复。";
        }

        private void label_info_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            label_info.Margin = new Thickness(471, 249, 0, 0);
            label_info.Height = 28;
            label_info.Width = 27;
            label_info.SetResourceReference(System.Windows.Controls.Label.ContentProperty, "lb-info");
        }
        #endregion
    }
}
