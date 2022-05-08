using System.Windows;
using System.Windows.Input;

namespace PMEditor.Views
{
    /// <summary>
    /// Interaction logic for ImageCapureWindow.xaml
    /// </summary>
    public partial class InfoSubWinodow : Window
    {
        public InfoSubWinodow()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                System.Console.WriteLine("UserControl_KeyDown_Shift_Control_Clicked");
            }
        }
    }
}
