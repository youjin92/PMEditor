using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.UserControls
{
    public class UCIncludeFileName : System.Windows.Controls.UserControl
    {
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(UCIncludeFileName), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPropertyChanged)));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

    }
}
