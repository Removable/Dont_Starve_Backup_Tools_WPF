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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Threading;

namespace Dont_Starve_Backup_Tools_WPF
{
    /// <summary>
    /// UpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : MetroWindow
    {
        private static UpdateWindow instance;
        private UpdateWindow()
        {
            InitializeComponent();
            MainWindow mw = MainWindow.GetInstance();
            this.Owner = mw;
            btn_cancel.Visibility = Visibility.Hidden;
            btn_sure.Visibility = Visibility.Hidden;
        }
        public static UpdateWindow GetInstance()
        {
            if (instance == null)
                instance = new UpdateWindow();
            return instance;
        }

        int result = 0;
        private void CheckUpdate()
        {
            result = DoWork.CheckUpdate();
            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case -1:
                            SetIco_CheckErr();
                            tblock_info.Text = "检查更新失败！";
                            break;
                        case 0:
                            SetIco_NoUpdate();
                            tblock_info.Text = "已经是最新版本！";
                            break;
                        case 1:
                            SetIco_NewVersion();
                            tblock_info.Text = "发现新版本，是否更新？";
                            btn_cancel.Visibility = Visibility.Visible;
                            btn_sure.Visibility = Visibility.Visible;
                            break;
                    }
                }));
            }).Start();            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid();
            grid.Width = 100;
            grid.Height = 100;
            ProgressRing pr = new ProgressRing();
            pr.IsActive = true;
            grid.Children.Add(pr);
            bd_1.Child = grid;

            Thread thread = new Thread(CheckUpdate);
            thread.IsBackground = true;
            thread.Start();
        }

        private void SetIco_NewVersion()
        {
            Grid grid = new Grid();
            grid.Width = 100;
            grid.Height = 100;
            Label lb = new Label();
            lb.SetResourceReference(Label.ContentProperty, "ico-newVersion");
            grid.Children.Add(lb);
            bd_1.Child = grid;
        }
        private void SetIco_NoUpdate()
        {
            Grid grid = new Grid();
            grid.Width = 100;
            grid.Height = 100;
            Label lb = new Label();
            lb.SetResourceReference(Label.ContentProperty, "ico-noUpdate");
            grid.Children.Add(lb);
            bd_1.Child = grid;
        }
        private void SetIco_CheckErr()
        {
            Grid grid = new Grid();
            grid.Width = 100;
            grid.Height = 100;
            Label lb = new Label();
            lb.SetResourceReference(Label.ContentProperty, "ico-checkErr");
            grid.Children.Add(lb);
            bd_1.Child = grid;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_sure_Click(object sender, RoutedEventArgs e)
        {
            if (tblock_info.Text == "发现新版本，是否更新？")
            {

            }
        }
    }
}
