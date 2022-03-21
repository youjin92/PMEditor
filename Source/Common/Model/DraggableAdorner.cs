using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
namespace Common.Model
{
    /// <summary>
    /// 드래그 가능 어도너
    /// </summary>
    public class DraggableAdorner : Adorner
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// 중심 포인트 오프셋
        /// </summary>
        public Point CenterPointOffset;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 렌더 사각형
        /// </summary>
        private Rect renderRectangle;

        /// <summary>
        /// 렌더 브러시
        /// </summary>
        private Brush renderBrush;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - DraggableAdorner(draggable)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="draggable">드래그 가능 객체</param>
        public DraggableAdorner(Draggable draggable) : base(draggable)
        {
            this.renderRectangle = new Rect(draggable.RenderSize);

            IsHitTestVisible = false;

            this.renderBrush = draggable.Background.Clone();

            CenterPointOffset = new Point(-this.renderRectangle.Width / 2, -this.renderRectangle.Height / 2);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 렌더링 처리하기 - OnRender(drawingContext)

        /// <summary>
        /// 렌더링 처리하기
        /// </summary>
        /// <param name="drawingContext">그리기 컨텍스트</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(this.renderBrush, new Pen(Brushes.Red,3), this.renderRectangle);
        }

        #endregion
    }
}

