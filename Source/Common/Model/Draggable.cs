using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static Common.CommonManager;

namespace Common.Model
{
    public class Draggable : UserControl
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Import
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region 커서 위치 구하기 - GetCursorPos(point)

        /// <summary>
        /// 커서 위치 구하기
        /// </summary>
        /// <param name="point">포인트</param>
        [DllImport("user32.dll")]
        private static extern void GetCursorPos(ref POINT point);

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        public string DragTagName = "TagName";
        public object DragPropertyobject = null;

        public DragDropEffects DragDropEffectsMode = DragDropEffects.Copy;

        /// <summary>
        /// 드래그 가능 어도너
        /// </summary>
        private DraggableAdorner draggableAdorner;

        /// <summary>
        /// 커서 포인트
        /// </summary>
        private POINT cursorPoint = new POINT();

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - Draggable()

        /// <summary>
        /// 생성자
        /// </summary>
        public Draggable()
        {
            MouseMove += UserControl_MouseMove;
            PreviewGiveFeedback += UserControl_PreviewGiveFeedback;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region 사용자 컨트롤 마우스 이동시 처리하기 - UserControl_MouseMove(sender, e)

        /// <summary>
        /// 사용자 컨트롤 마우스 이동시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dataObject = new DataObject(DragTagName , DragPropertyobject);

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);

                draggableAdorner = new DraggableAdorner(this);

                adornerLayer.Add(draggableAdorner);

                DragDrop.DoDragDrop(this, dataObject, DragDropEffectsMode);

                adornerLayer.Remove(draggableAdorner);
            }
        }

        #endregion
        #region 사용자 컨트롤 프리뷰 피드백 주기 - UserControl_PreviewGiveFeedback(sender, e)

        /// <summary>
        /// 사용자 컨트롤 프리뷰 피드백 주기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            GetCursorPos(ref cursorPoint);

            Point point = PointFromScreen(cursorPoint.GetPoint(draggableAdorner.CenterPointOffset));

            draggableAdorner.Arrange(new Rect(point, draggableAdorner.DesiredSize));
        }

        #endregion
    }

    /// <summary>
    /// 포인트
    /// </summary>
    public struct POINT
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// X
        /// </summary>
        public int X;

        /// <summary>
        /// Y
        /// </summary>
        public int Y;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - POINT(x, y)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion
        #region 생성자 - POINT(x, y)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public POINT(double x, double y)
        {
            X = (int)x;
            Y = (int)y;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 포인트 구하기 - GetPoint(xOffset, yOffet)

        /// <summary>
        /// 포인트 구하기
        /// </summary>
        /// <param name="offsetX">오프셋 X</param>
        /// <param name="offsetY">오프셋 Y</param>
        /// <returns>포인트</returns>
        public Point GetPoint(double offsetX = 0, double offsetY = 0)
        {
            return new Point(X + offsetX, Y + offsetY);
        }

        #endregion
        #region 포인트 구하기 - GetPoint(offsetPoint)

        /// <summary>
        /// 포인트 구하기
        /// </summary>
        /// <param name="offsetPoint">오프셋 포인트</param>
        /// <returns>포인트</returns>
        public Point GetPoint(Point offsetPoint)
        {
            return new Point(X + offsetPoint.X, Y + offsetPoint.Y);
        }

        #endregion
    }
}
