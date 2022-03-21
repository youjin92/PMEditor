using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Common.Model
{
    /// <summary>
    /// Dropable
    /// </summary>
    public partial class Dropable : UserControl
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public
        #region 이벤트
        public event DragEventHandler DragEnterAction;
        public event DragEventHandler DropAction;
        public event DragEventHandler DragLeaveAction;


        #endregion


        #region 생성자 - Dropable()

        /// <summary>
        /// 생성자
        /// </summary>
        public Dropable()
        {
            DragEnter += UserControl_DragEnter;
            Drop += UserControl_Drop;
            DragLeave += UserControl_DragLeave;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 사용자 컨트롤 드래그 ENTER 처리하기 - UserControl_DragEnter(sender, e)

        /// <summary>
        /// 사용자 컨트롤 드래그 ENTER 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            if (DragEnterAction != null)
                DragEnterAction(this, e);

        }

        #endregion
        #region 사용자 컨트롤 DROP 처리하기 - UserControl_Drop(sender, e)

        /// <summary>
        /// 사용자 컨트롤 DROP 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (DropAction != null)
                DropAction(this, e);
        }

        #endregion
        #region 사용자 컨트롤 드래그 이탈시 처리하기 - UserControl_DragLeave(sender, e)

        /// <summary>
        /// 사용자 컨트롤 드래그 이탈시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_DragLeave(object sender, DragEventArgs e)
        {
            if (DragLeaveAction != null)
                DragLeaveAction(this, e);
        }

        #endregion
    }
}
