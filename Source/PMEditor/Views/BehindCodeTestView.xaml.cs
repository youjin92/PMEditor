using System.Windows.Controls;
using System.Windows.Input;

namespace PMEditor.Views
{
    /// <summary>
    /// Interaction logic for BehindCodeTestView
    /// </summary>
    public partial class BehindCodeTestView : UserControl
    {
        public BehindCodeTestView()
        {
            InitializeComponent();
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                System.Console.WriteLine("UserControl_KeyDown_Shift_Control_Clicked");
            }
        }

        private void UserControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TreeView_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
