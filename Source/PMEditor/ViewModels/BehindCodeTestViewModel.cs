using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class BehindCodeTestViewModel : BindableBase
    {
        public BehindCodeTestViewModel()
        {

        }

        private DelegateCommand _ClickCommand;
        public DelegateCommand ClickCommand =>
            _ClickCommand ?? (_ClickCommand = new DelegateCommand(ExecuteClickCommand));
        void ExecuteClickCommand()
        {
            Console.WriteLine("ExecuteClickCommand");
        }
    }
}
