using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class UITestViewModel : BindableBase
    {
        public string NameProperty { get; set; } = "TEST Name";
        public string TextBoxText { get; set; } = "TEST Name";
        public string InputText { get; set; } = "InputText";

        private DelegateCommand _ChangeTextCommand;
        public DelegateCommand ChangeTextCommand => _ChangeTextCommand ?? (_ChangeTextCommand = new DelegateCommand(ExecuteChangeTextCommand));
        void ExecuteChangeTextCommand()
        {
            InputText = TextBoxText;
        }
    }
}
