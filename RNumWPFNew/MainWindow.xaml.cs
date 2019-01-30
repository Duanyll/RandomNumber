using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.IO;
using System.Windows.Threading;

namespace RNumWPFNew
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        List<string> Names = new List<string>();
        List<string> OrginNames = new List<string>();
        Random rand = new Random();
        int NowIdx = 0;
        bool Running = false;
        bool FileOpened = false;
        int Counter = 1;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.Speed);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void ButtonStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (!FileOpened)
            {
                snakeBar.MessageQueue.Enqueue("未选择或打开失败花名册文件，请检查设置");
                return;
            }
            if (Running)
            {
                timer.Stop();
                ButtonStartStop.Content = string.Format("开始第{0}个人", ++Counter);
                Running = false;
            }
            else
            {
                //Names.Clear();
                if (Names.Count <= 1)
                {
                    snakeBar.MessageQueue.Enqueue("所有人都被选过了", "清除记录", () => RefreshNames());
                    return;
                }
                if (Properties.Settings.Default.AvoidSlected == true && Counter > 1)
                {
                    Names.RemoveAt(NowIdx);
                    WrapPanelName.Children.RemoveAt(NowIdx);
                }
                //snakeBar.MessageQueue.Enqueue(string.Format("正在抽取第{0}个人", ++Counter));
                timer.Start();
                ButtonStartStop.Content = "停止";
                Running = true;
            }
        }

        private void OpenFile()
        {
            if (File.Exists(Properties.Settings.Default.DefaultNameBook))
            {
                OrginNames.Clear();
                WrapPanelName.Children.Clear();
                string[] lines = File.ReadAllLines(Properties.Settings.Default.DefaultNameBook);
                foreach (string i in lines)
                {
                    OrginNames.Add(i);
                }
                RefreshNames();
                FileOpened = true;
            }
        }

        void RefreshNames()
        {
            timer.Stop();
            Running = false;
            Names = new List<string>(OrginNames);
            WrapPanelName.Children.Clear();
            GC.Collect();
            foreach (string i in Names)
            {
                WrapPanelName.Children.Add(new NameLabel(i));
            }
            Counter = 1;
            snakeBar.MessageQueue.Enqueue("已设置名字");
            ButtonStartStop.Content = "开始第1个人";
        }

        int LastID = 0;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int now = GetNextIdx();
            TextBlockName.Text = Names[now];
            if (LastID < WrapPanelName.Children.Count)
                ((NameLabel)WrapPanelName.Children[LastID]).StopHighLight();
            ((NameLabel)WrapPanelName.Children[now]).HighLight();
            LastID = now;
            var currentScrollPosition = SViewer1.VerticalOffset;
            var point = new Point(0, currentScrollPosition);
            var targetPosition = WrapPanelName.Children[now].TransformToVisual(SViewer1).Transform(point);
            SViewer1.ScrollToVerticalOffset(targetPosition.Y - rand.Next(200));
        }

        private int GetNextIdx()
        {
            NowIdx = rand.Next() % Names.Count;
            return NowIdx;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            RefreshNames();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt文件，一排一个名字|*.txt";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.DefaultNameBook = dialog.FileName;
                Properties.Settings.Default.Save();
                OpenFile();
            }
        }

        private void ButtonClear_Click_1(object sender, RoutedEventArgs e)
        {
            RefreshNames();
        }

        private void ButtonClearCounter_Click(object sender, RoutedEventArgs e)
        {
            Counter = 1;
            snakeBar.MessageQueue.Enqueue("已重置计数器");
            ButtonStartStop.Content = "开始第1个人";
        }
    }
}
