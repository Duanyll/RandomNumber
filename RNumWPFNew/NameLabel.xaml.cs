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

namespace RNumWPFNew
{
    /// <summary>
    /// NameLabel.xaml 的交互逻辑
    /// </summary>
    public partial class NameLabel : UserControl
    {
        //readonly Color DEFAULT_COLOR = new Color();
        //readonly Color HIGHLIGHT_COLOR = new Color();
        public NameLabel()
        {
            InitializeComponent();
        }

        public NameLabel(string t)
        {
            InitializeComponent();
            TextBlockName.Text = t;
            TextBlockName.Foreground = new SolidColorBrush(SystemColors.ControlTextColor);
        }

        public void HighLight()
        {
            border.Background = new SolidColorBrush(SystemColors.HighlightColor);
            TextBlockName.Foreground = SystemColors.HighlightTextBrush;
        }

        public void StopHighLight()
        {
            border.Background = SystemColors.ControlLightBrush;
            TextBlockName.Foreground = SystemColors.ControlTextBrush;
        }
    }
}
