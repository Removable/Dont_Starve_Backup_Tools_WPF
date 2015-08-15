using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Dont_Starve_Backup_Tools_WPF
{    
    public class DoWork
    {
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 自动扫描方法
        /// </summary>
        /// <param name="obj">要扫描的文件夹</param>
        /// <param name="tb_filePath">扫描完成后设置的文本框</param>
        public static void Scanner(DirectoryInfo obj, TextBox tb_filePath)
        {
            FileSystemInfo myFSI = obj as FileSystemInfo;
            if (!myFSI.Exists)
            {
                //System.Windows.MessageBox.Show("文件夹不存在！");
                return;
            }
            else
            {
                DirectoryInfo dir = myFSI as DirectoryInfo;

                try
                {
                    FileSystemInfo[] files = dir.GetFileSystemInfos();
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo fi = files[i] as FileInfo;
                        if (fi != null)
                        {
                            if (StaticResource.filesName.Contains(fi.Name))
                            {
                                tb_filePath.Text = fi.DirectoryName;
                            }
                        }
                        else
                        {
                            DirectoryInfo di = files[i] as DirectoryInfo;
                            Scanner(di, tb_filePath);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>  
        /// 备份的静态方法  
        /// </summary>  
        /// <param name="filePath">饥荒存档路径</param>  
        /// <param name="backupPath">用户设置的备份路径</param>
        /// <returns>本次存档</returns>
        public static string BackUp(string filePath, string backupPath)
        {
            string newDirectory = null;
            try
            {
                string[] files = Directory.GetFiles(@filePath);
                foreach (var item in files)
                {
                    newDirectory = backupPath + @"\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    Directory.CreateDirectory(newDirectory);
                    File.Copy(item, newDirectory + @"\" + Path.GetFileName(item), true);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("备份失败！");
            }
            return newDirectory;
        }

        /// <summary>
        /// 强制备份已有存档到备份目录下的LatestFiles文件夹
        /// </summary>
        public static void CompulsoryBackup()
        {
            try
            {
                string filePath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
                string backupPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置");
                string[] files = Directory.GetFiles(filePath);
                if (!Directory.Exists(backupPath + @"\LatestFiles"))
                    Directory.CreateDirectory(backupPath + @"\LatestFiles");
                foreach (var item in files)
                {
                    File.Copy(item, backupPath + @"\LatestFiles\" + item.Substring(item.LastIndexOf(@"\") + 1), true);
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 一键转移备份的方法
        /// </summary>
        /// <param name="oldPath">现有的备份路径</param>
        /// <param name="newPath">要转移到的路径</param>
        public static void TransferBackup(string oldPath, string newPath)
        {
            try
            {
                string[] files = Directory.GetFiles(@oldPath);
                string newDirectory = null;
                foreach (var item in files)
                {
                    newDirectory = newPath;
                    Directory.CreateDirectory(newDirectory);
                    File.Copy(item, newDirectory + @"\" + Path.GetFileName(item), true);
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 对下次备份进行倒计时的方法
        /// </summary>
        /// <param name="tb_min">显示“分”的TextBlock</param>
        /// <param name="tb_sec">显示“秒”的TextBlock</param>
        /// <param name="min">备份间隔</param>
        /// <param name="filePath">饥荒存档路径</param>  
        /// <param name="backupPath">用户设置的备份路径</param>
        public static void CountDown(TextBlock tb_min, TextBlock tb_sec, int min, string filePath, string backupPath)
        {
            if (tb_sec.Text == "00")
            {
                if (tb_min.Text == "00")
                {
                    BackUp(filePath, backupPath);
                    tb_min.Text = min.ToString().PadLeft(2, '0');
                    tb_sec.Text = "00";
                }
                else
                {
                    tb_sec.Text = "59";
                    tb_min.Text = (int.Parse(tb_min.Text) - 1).ToString().PadLeft(2, '0');
                }
            }
            else
                tb_sec.Text = (int.Parse(tb_sec.Text) - 1).ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 存档还原方法
        /// </summary>
        /// <param name="name">要恢复的备份名</param>
        public static void Restore(string name)
        {
            try
            {
                string[] files = Directory.GetFiles(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\" + name);
                foreach (var item in files)
                {
                    File.Copy(item, MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置") + @"\" + Path.GetFileName(item), true);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("还原失败！");
            }
        }

        /// <summary>
        /// 删除对应备份的方法
        /// </summary>
        /// <param name="name">要删除的备份名</param>
        public static void DeletBackUp(string name)
        {
            try
            {
                string direPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\" + name;
                string[] files = Directory.GetFiles(direPath);
                foreach (var item in files)
                {
                    File.Delete(item);
                }
                Directory.Delete(direPath);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("删除失败！");
            }
        }

        /// <summary>
        /// 一键转移所有备份
        /// </summary>
        /// <param name="newPath">要转移到的新路径</param>
        public static void CopyAllBackup(string newPath)
        {
            try
            {
                string[] directories = Directory.GetDirectories(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置"));
                foreach (var item in directories)
                {
                    string itemName = item.Substring(item.LastIndexOf(@"\") + 1);
                    if (itemName.Length != 19 && itemName.Length != 11)
                        continue;
                    if (item.Length == 19)
                        if (itemName.Substring(4, 1) != "-" || itemName.Substring(7, 1) != "-")
                            continue;
                    if (item.Length == 11)
                        if (item != "LatestFiles")
                            continue;
                    TransferBackup(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\" + itemName, @newPath + @"\" + itemName);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("转移失败！");
            }
        }

        /// <summary>
        /// 还原时，对现有存档进行强制备份的方法
        /// </summary>
        public static void OldFileRestore()
        {
            try
            {
                if (!Directory.Exists(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\LatestFiles"))
                {
                    System.Windows.MessageBox.Show("未发现自动备份的旧存档！");
                    return;
                }
                string[] files = Directory.GetFiles(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "BackupPath", "未设置") + @"\LatestFiles");
                if (files == null || files.Length == 0)
                {
                    System.Windows.MessageBox.Show("未发现自动备份的旧存档！");
                    return;
                }
                string tempPath = MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置");
                string[] temp = Directory.GetFiles(tempPath);
                foreach (var item in temp)
                {
                    File.Delete(item);
                }
                foreach (var item in files)
                {
                    File.Copy(item, MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Path", "FilePath", "未设置") + @"\" + item.Substring(item.LastIndexOf(@"\") + 1), true);
                }
                System.Windows.MessageBox.Show("恢复成功！");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("失败！");
            }
        }


        /// <summary>
        /// 检测更新的方法
        /// </summary>
        /// <returns></returns>
        public static int CheckUpdate()
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string str = "";

            int i = 0;
            if (!InternetGetConnectedState(out i, 0))
            {
                return -1;
            }
            try
            {
                request = WebRequest.Create(new Uri("http://dsbackuptool-toolupdate.stor.sinaapp.com/ds-versionInfo.txt")) as HttpWebRequest;
                request.Timeout = 10 * 1000;
                request.Method = "GET";
                request.ContentType = "text/plain";

                response = request.GetResponse() as HttpWebResponse;
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.Default);
                str = sr.ReadToEnd();
                string temp = str.Substring(1, 5);
                double netVersion = double.Parse(str.Substring(1, 5));
                double thisVersion = double.Parse(MyProfiles.INIGetStringValue(StaticResource.iniFilePath, "Update", "Version", "0.000"));
                if (thisVersion < netVersion)
                {
                    StaticResource.DownloadUri = str.Substring(7);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
        //public static int DownloadNewVersion(string downloadUri)
        //{  
            
        //}
    }
}
