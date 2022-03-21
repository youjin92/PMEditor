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

namespace Common.Model
{
    /// <summary>
    /// DropItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DropItem : Dropable
    {
        public DropItem()
        {
            InitializeComponent();

            DragEnterAction += new DragEventHandler(UserControl_DragEnter);
            DropAction += new DragEventHandler(UserControl_Drop);
            DragLeave += new DragEventHandler(UserControl_DragLeave);
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            this.rectangle.Stroke = Brushes.LightBlue;
            this.rectangle.StrokeDashArray = new DoubleCollection() { 4, 2 };
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            this.rectangle.Stroke = Brushes.Black;
            this.rectangle.StrokeDashArray = null;

            string dragItem = e.Data.GetData("DragItem").ToString(); ;
            TxtBlock.Text = dragItem;
        }

        private void UserControl_DragLeave(object sender, DragEventArgs e)
        {
            this.rectangle.Stroke = Brushes.Black;
            this.rectangle.StrokeDashArray = null;
        }
    }
}
