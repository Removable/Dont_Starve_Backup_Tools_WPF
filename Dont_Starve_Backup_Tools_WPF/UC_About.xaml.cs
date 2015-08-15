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

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// UC_About.xaml 的交互逻辑
    /// </summary>
    public partial class UC_About : UserControl
    {
        private static UC_About instance = null;
        private UC_About()
        {
            InitializeComponent();
            ShowInfo();
        }
        public static UC_About GetInstance()
        {
            if (instance == null)
                instance = new UC_About();
            return instance;
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo();
        }

        private void ShowInfo()
        {
            btn_about.Foreground = Brushes.White;
            btn_updateLog.Foreground = Brushes.Gray;
            tb_show.Text = "饥荒存档备份工具\r\n由 可更换零部件 设计、编写，希望能给广大饥荒玩家带来一丝方便~\r\n\r\n在软件编写过程中，\r\n得到了很多朋友的帮助和支持，在此表示衷心的感谢！\r\n\r\n一些刚接触饥荒不久的朋友可以\r\n点击下方链接看一看路叔的新手教学视频系列，质量还是比较不错的~\r\n同时也希望大家关注一下游戏主播——路叔的斗鱼直播间，谢谢大家的支持~";
            tb_show.TextAlignment = TextAlignment.Center;
            btn_jxsp.Visibility = Visibility.Visible;
            btn_zbj.Visibility = Visibility.Visible;
            btn_checkUpdate.Visibility = Visibility.Hidden;
        }
        private void btn_jxsp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://v.youku.com/v_show/id_XODE0MjI5ODEy.html?f=22991311");
        }

        private void btn_zbj_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.douyutv.com/67524?fromuid=1117986");
        }

        private void OpenText()
        {

            TextReader tr = null;
            try
            {
                tr = new StreamReader(StaticResource.updatelogFilePath, Encoding.Default);
                this.tb_show.Text = tr.ReadToEnd();
            }
            catch (Exception oex)
            {
                MessageBox.Show("打开异常，错误信息：" + oex.Message);
            }
            finally
            {
                if (tr != null)
                    tr.Close();
            }
        }
        private void btn_updateLog_Click(object sender, RoutedEventArgs e)
        {
            OpenText();
            tb_show.TextAlignment = TextAlignment.Left;
            btn_about.Foreground = Brushes.Gray;
            btn_updateLog.Foreground = Brushes.White;
            btn_jxsp.Visibility = Visibility.Hidden;
            btn_zbj.Visibility = Visibility.Hidden;
            btn_checkUpdate.Visibility = Visibility.Visible;
        }

        private void btn_checkUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow uw = UpdateWindow.GetInstance();
            uw.ShowDialog();
        }
    }
}
