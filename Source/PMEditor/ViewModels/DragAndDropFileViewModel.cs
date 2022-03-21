using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class DragAndDropFileViewModel : BindableBase
    {
        #region 프로퍼티
        public string FileOutputText { get; set; } = "test111";
        #endregion

        #region 커멘드
        private DelegateCommand _DropCommand;
        public DelegateCommand DropCommand =>_DropCommand ?? (_DropCommand = new DelegateCommand(ExecuteDropCommand));
        void ExecuteDropCommand()
        {
            Console.WriteLine("ExecuteDropCommand");
        }

        private DelegateCommand _DragEnterCommand;
        public DelegateCommand DragEnterCommand =>_DragEnterCommand ?? (_DragEnterCommand = new DelegateCommand(ExecuteDragEnterCommand));
        void ExecuteDragEnterCommand()
        {
            Console.WriteLine("ExecuteDragEnterCommand");
        }

        private DelegateCommand _DragLeaveCommand;
        public DelegateCommand DragLeaveCommand =>_DragLeaveCommand ?? (_DragLeaveCommand = new DelegateCommand(ExecuteDragLeaveCommand));
        void ExecuteDragLeaveCommand()
        {
            Console.WriteLine("ExecuteDragLeaveCommand");
        }

        private DelegateCommand _PreviewMouseWheelCommand;
        public DelegateCommand PreviewMouseWheelCommand =>_PreviewMouseWheelCommand ?? (_PreviewMouseWheelCommand = new DelegateCommand(ExecutePreviewMouseWheelCommand));
        void ExecutePreviewMouseWheelCommand()
        {
            Console.WriteLine("ExecutePreviewMouseWheelCommand");
        }

        private DelegateCommand _KeyDownCommand;
        public DelegateCommand KeyDownCommand =>_KeyDownCommand ?? (_KeyDownCommand = new DelegateCommand(ExecuteKeyDownCommand));
        void ExecuteKeyDownCommand()
        {
            Console.WriteLine("ExecuteKeyDownCommand");
        }

        private DelegateCommand _LoadedCommand;
        public DelegateCommand LoadedCommand =>_LoadedCommand ?? (_LoadedCommand = new DelegateCommand(ExecuteLoadedCommand));
        void ExecuteLoadedCommand()
        {
            Console.WriteLine("ExecuteLoadedCommand");
        }
        #endregion
    }
}
