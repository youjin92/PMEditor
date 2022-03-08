using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class LoadingWindowViewModel : BindableBase
    {
        public LoadingWindowViewModel()
        {

        }

        public string LoadingText { get; set; } = "엑셀파일 정보 불러오는중...";
        public string Name { get; set; } = "PMEditor";
        public string Content { get; set; } = "Loading...";
    }
}
